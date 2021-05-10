using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbModels;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public class InspectionRepository : IInspectionRepository
    {
        public async Task<InspectionModel> CreateInspection(InspectionModel model)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = model.Map();
                var assessorIds = inspection.Assessors.Select(x => x.Id);
                inspection.Assessors = context.Employees.Where(x => assessorIds.Contains(x.Id)).ToList();
                
                context.Inspections.Add(inspection);

                await context.SaveChangesAsync();

                return model;
            }
        }

        public async Task AddInspectionDocument(Guid inspectionId, string documentName, byte[] document)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = context.Inspections.Include(x => x.Documents).FirstOrDefault(x => x.Id == inspectionId);

                if (inspection == null)
                {
                    throw new Exception("Inspection not found.");
                }

                if (inspection.Documents.Any(x => x.Name == documentName))
                {
                    throw new Exception("Document with same name already exists");
                }

                context.Documents.Add(new Document
                {
                    Id = Guid.NewGuid(),
                    InspectionId = inspectionId,
                    Name = documentName,
                    Data = document
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<DocumentModel> GetInspectionDocument(Guid documentId)
        {
            using (var context = new ISControlDbContext())
            {
                var document = await context.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

                if (document == null)
                {
                    throw new Exception("Document not found");
                }

                return document.Map();
            }
        }

        public async Task<Page<BriefDocumentModel>> GetDocumentList(Guid inspectionId, int take, int skip)
        {
            using (var context = new ISControlDbContext())
            {
                var documents = context.Documents.Where(x => x.InspectionId == inspectionId);

                return new Page<BriefDocumentModel>
                {
                    Items = documents
                        .OrderBy(x => x.Name)
                        .Skip(skip)
                        .Take(take)
                        .Select(x => new BriefDocumentModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList(),
                    Total = documents.Count()
                };
            }
        }

        public async Task DeleteDocument(Guid documentId)
        {
            using (var context = new ISControlDbContext())
            {
                var document = await context.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

                if (document == null)
                {
                    throw new Exception("Document not found");
                }

                context.Documents.Remove(document);

                await context.SaveChangesAsync();
            }
        }
    }
}