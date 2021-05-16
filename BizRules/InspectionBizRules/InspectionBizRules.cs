using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BizRules.UsersBizRules;
using Common.Models;
using Common.Models.Enums;
using Common.Models.RequestModels;
using DataAccess.DataAccess.CompanyRepository;
using DataAccess.DataAccess.EvaluationRepository;
using DataAccess.DataAccess.InspectionRepository;
using DataAccess.DataAccess.UserRepository;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Models;

namespace BizRules.InspectionBizRules
{
    public class InspectionBizRules : IInspectionBizRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUsersBizRules _usersBizRules;

        public InspectionBizRules(
            IUserRepository userRepository, 
            IInspectionRepository inspectionRepository, 
            ICompanyRepository companyRepository, 
            IEvaluationRepository evaluationRepository, 
            IUsersBizRules usersBizRules)
        {
            _userRepository = userRepository;
            _inspectionRepository = inspectionRepository;
            _companyRepository = companyRepository;
            _evaluationRepository = evaluationRepository;
            _usersBizRules = usersBizRules;
        }

        public async Task<InspectionModel> CreateInspection(Guid contractorId, Guid userId)
        {
            var user = _userRepository.GetUser(userId);
            var customer = await _companyRepository.GetCompany(user.CompanyId.Value);
            var contractor = await _companyRepository.GetCompany(contractorId);

            if (customer == null)
            {
                throw new Exception("Company not found.");
            }

            if (customer.Role != CompanyRole.Customer)
            {
                throw new Exception("Company must be Customer.");
            }

            if (contractor.Role != CompanyRole.Contractor)
            {
                throw new Exception("Contractor company must be Contractor.");
            }

            return await _inspectionRepository.CreateInspection(contractorId, customer.Id);
        }

        public async Task<InspectionModel> StartInspection(Guid inspectionId, List<Guid> assessorIds)
        {
            return await _inspectionRepository.StartInspection(inspectionId, assessorIds);
        }

        public async Task<InspectionModel> GetInspection(Guid inspectionId)
        {
            return await _inspectionRepository.GetInspection(inspectionId);
        }

        public async Task<List<EventModel>> AddEvent(Guid inspectionId, EventModel model)
        {
            return await _inspectionRepository.AddInspectionEvent(inspectionId, model);
        }

        public async Task<List<EventModel>> DeleteEvent(Guid inspectionId, Guid eventId)
        {
            return await _inspectionRepository.DeleteInspectionEvent(inspectionId, eventId);
        }

        public async Task AddInspectionDocument(CreateInspectionDocumentRequest request)
        {
            await _inspectionRepository.AddInspectionDocument(request.InspectionId, request.DocumentName,
                request.DocumentData);
        }

        public async Task<DocumentModel> GetInspectionDocument(Guid documentId)
        {
            return await _inspectionRepository.GetInspectionDocument(documentId);
        }

        public async Task<Page<BriefDocumentModel>> GetInspectionDocuments(Guid inspectionID, int take, int skip)
        {
            return await _inspectionRepository.GetDocumentList(inspectionID, take, skip);
        }

        public async Task DeleteDocument(Guid documentId)
        {
            await _inspectionRepository.DeleteDocument(documentId);
        }

        public async Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId, int take, int skip, bool? onlySet, bool? positive, string name)
        {
            return await _evaluationRepository.GetEvaluations(inspectionId, take, skip, onlySet, positive, name);
        }

        public async Task SetEvaluation(Guid inspectionId, int reqId, double? score, string description) 
        {
            var inspection = await GetInspection(inspectionId);
            if (inspection.Status != InspectionStatus.InProgress)
            {
                throw new Exception("Wrong status to set evaluations.");
            }

            await _evaluationRepository.SetEvaluation(inspectionId, reqId, score, description);
        }

        public async Task UpdateInspectionStatus(Guid inspectionId, InspectionStatus status)
        {
            await _inspectionRepository.UpdateInspectionStatus(inspectionId, status);
            if (status == InspectionStatus.Finished)
            {
                var inspectionEvaluations = (await GetEvaluations(inspectionId, int.MaxValue, 0, null, null, null)).Items;
                var digitScore = Math.Min(CalculateFirstCompositeIndex(inspectionEvaluations),
                    CalculateSecondCompositeIndex(inspectionEvaluations));
                var score = ResolveScore(digitScore);
                await _inspectionRepository.SetInspectionFinalScore(inspectionId, score, digitScore);
            }
        }

        public async Task ApproveInspection(Guid userId, Guid inspectionId)
        {
            await _inspectionRepository.ApproveInspection(userId, inspectionId);
        }

        public async Task<byte[]> GenerateFirstForm(Guid inspectionId)
        {
            var tableLines =  (await GetEvaluations(inspectionId, int.MaxValue, 0, null, null, null))
                .Items
                .SelectMany(x => x.Requirements)
                .Select(x => new FirstFormTableLine
                {
                    Id = $"П. {x.Id}",
                    Description = x.Description,
                    Score = x.Score.Value.ToString(),
                    ScoreDescription = x.EvaluationDescription
                })
                .ToList();
            return GenerateFirstTable(tableLines);
        }

        public async Task<byte[]> GenerateSecondForm(Guid inspectionId)
        {
            var inspectionEvaluations = (await GetEvaluations(inspectionId, int.MaxValue, 0, null, null, null)).Items;
            var lines = new List<(string, string)>
            {
                ("Обобщающий показатель 1", CalculateFirstCompositeIndex(inspectionEvaluations).ToString()),
                ("Обобщающий показатель 2", CalculateSecondCompositeIndex(inspectionEvaluations).ToString())
            };

            return GenerateSecondTable(lines);
        }

        public async Task<string> ResolveFormName(Guid inspectionId, string formNumber)
        {
            var inspection = await GetInspection(inspectionId);

            return $"Инсп{inspection.StartDate.Value:d}_Форма_{formNumber}_{DateTime.Now:d}.docx";
        }

        public async Task<InspectionModel> GetLastOrderedInspection(Guid userId)
        {
            var companyId = _userRepository.GetUser(userId).CompanyId.Value;
            var inspectionId = await _inspectionRepository.GetLastOrderedCompanyInspectionId(companyId);

            if (inspectionId == new Guid())
            {
                return null;
            }

            return await _inspectionRepository.GetInspection(inspectionId);
        }

        public async Task<Score?> PredictFinalScore(Guid userId)
        {
            var inspectionsScores = (await _usersBizRules.GetMyInspections(userId, 1000, 0)).Items.Select(x => x.FinalDigitScore).Select(x => x.Value).ToList();
            var prediction = PredictDigitScore(inspectionsScores);
            return prediction == null ? (Score?) null : ResolveScore(prediction.Value);
        }

        private double CalculateFirstCoefficient(List<CategoryModel> evaluations)
        {
            var actualEvaluationZeros = evaluations.SelectMany(x => x.Requirements).Count(x => x.Score.Value == 0);

            if (actualEvaluationZeros == 0)
            {
                return 1;
            }

            return actualEvaluationZeros < 11 ? 0.85 : 0.7;
        }

        private double CalculateSecondCoefficient(List<CategoryModel> evaluations)
        {
            var actualEvaluationZeros = evaluations.SelectMany(x => x.Requirements).Count(x => x.Score.Value == 0);

            if (actualEvaluationZeros == 0)
            {
                return 1;
            }

            return actualEvaluationZeros < 6 ? 0.85 : 0.7;
        }

        private double CalculateFirstCompositeIndex(List<CategoryModel> models)
        {
            var actualCategories = models.Where(x => Convert.ToInt32(x.Number.Split('.')[1]) < 11).ToList();

            return Math.Round(actualCategories
                       .SelectMany(x => x.Requirements)
                       .Select(x => x.Score.Value).Average() *
                   CalculateFirstCoefficient(actualCategories), 2);
        }

        private double CalculateSecondCompositeIndex(List<CategoryModel> models)
        {
            var actualCategories = models.Where(x => Convert.ToInt32(x.Number.Split('.')[1]) >= 11).ToList();

            return Math.Round(actualCategories
                       .SelectMany(x => x.Requirements)
                       .Select(x => x.Score.Value).Average() *
                   CalculateSecondCoefficient(actualCategories), 2);
        }

        private Score ResolveScore(double score)
        {
            if (score >= 0.85)
            {
                return Score.Good;
            }

            if (score >= 0.7)
            {
                return Score.Passable;
            }

            return score >= 0.5 ? Score.Doubtful : Score.Inefficient;
        }

        private byte[] GenerateFirstTable(List<FirstFormTableLine> lines)
        {
            byte[] result;
            using (var stream = new MemoryStream())
            {
                using (var doc =
                    WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
                {
                    var table = new Table();

                    // Create a TableProperties object and specify its border information.
                    var tblProp = new TableProperties(
                        new TableBorders(
                            new TopBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new BottomBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new LeftBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new RightBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new InsideHorizontalBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new InsideVerticalBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            }
                        )
                    );
                    table.AppendChild(tblProp);


                    table.Append(CreateFirstFormRow(new FirstFormTableLine()
                    {
                        Id = "№",
                        Description = "Формулировка требования к обеспечению защиты информации при осуществлении переводов денежных средств",
                        Score = "Оценка выполнения требования",
                        ScoreDescription = "Факторы, учитываемые при оценке, краткая формулировка обоснования выставленной оценки",
                    }));

                    foreach (var modelLine in lines)
                    {
                        table.Append(CreateFirstFormRow(modelLine));
                    }

                    var mainPart = doc.AddMainDocumentPart();

                    mainPart.Document = new Document(
                        new Body());

                    // Append the table to the document.
                    doc.MainDocumentPart.Document.Body.Append(table);


                    doc.MainDocumentPart.Document.Save();
                }

                result = stream.ToArray();
            }

            return result;
        }

        private byte[] GenerateSecondTable(List<(string, string)> lines)
        {
            byte[] result;
            using (var stream = new MemoryStream())
            {
                using (var doc =
                    WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
                {
                    var table = new Table();

                    // Create a TableProperties object and specify its border information.
                    var tblProp = new TableProperties(
                        new TableBorders(
                            new TopBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new BottomBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new LeftBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new RightBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new InsideHorizontalBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            },
                            new InsideVerticalBorder()
                            {
                                Val =
                                    new EnumValue<BorderValues>(BorderValues.Apples),
                                Size = 4
                            }
                        )
                    );
                    table.AppendChild(tblProp);

                    table.Append(CreateSecondFormRow(("Обобщающий показатель", "Значение обобщающего показателя")));

                    foreach (var modelLine in lines)
                    {
                        table.Append(CreateSecondFormRow(modelLine));
                    }

                    var mainPart = doc.AddMainDocumentPart();

                    mainPart.Document = new Document(
                        new Body());

                    // Append the table to the document.
                    doc.MainDocumentPart.Document.Body.Append(table);


                    doc.MainDocumentPart.Document.Save();
                }

                result = stream.ToArray();
            }

            return result;
        }

        private static TableRow CreateFirstFormRow(FirstFormTableLine modelLine)
        {
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc1.Append(new Paragraph(new Run(new Text(text: modelLine.Id))));
            tr.Append(tc1);

            TableCell tc2 = new TableCell();
            tc2.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc2.Append(new Paragraph(new Run(new Text(modelLine.Description))));
            tr.Append(tc2);

            TableCell tc3 = new TableCell();
            tc3.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc3.Append(new Paragraph(new Run(new Text(modelLine.Score))));
            tr.Append(tc3);

            TableCell tc4 = new TableCell();
            tc4.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc4.Append(new Paragraph(new Run(new Text(modelLine.ScoreDescription))));
            tr.Append(tc4);

            return tr;
        }

        private static TableRow CreateSecondFormRow((string, string) modelLine)
        {
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc1.Append(new Paragraph(new Run(new Text(text: modelLine.Item1))));
            tr.Append(tc1);

            TableCell tc2 = new TableCell();
            tc2.Append(new TableCellProperties(
                new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
            tc2.Append(new Paragraph(new Run(new Text(modelLine.Item2))));
            tr.Append(tc2);

            return tr;
        }

        private double? PredictDigitScore(List<double> scores)
        {
            if (scores.Count <= 3)
            {
                return null;
            }

            var alpha = 0.5;
            var sma = (scores[0] + scores[1] + scores[2]) / 3;
            var ema = alpha * scores[3] + (1 - alpha) * sma;

            if (scores.Count > 4)
            {
                for (var i = 4; i < scores.Count; i++)
                {
                    ema = alpha * scores[i] + (1 - alpha) * ema;
                }
            }

            return ema;
        }
    }
}