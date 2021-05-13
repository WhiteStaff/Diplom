using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Enums;
using Models;

namespace DataAccess.DataAccess.EvaluationRepository
{
    public class EvaluationRepository : IEvaluationRepository
    {
        public async Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId,
            int take,
            int skip,
            bool? onlySet,
            bool? positive, string name)
        {
            using (var context = new ISControlDbContext())
            {
                var evaluations = context.Inspections
                    .AsQueryable()
                    .Include(x => x.Evaluations)
                    .First(x => x.Id == inspectionId)
                    .Evaluations
                    .ToDictionary(x => x.RequirementId, x => x);

                Func<RequirementModel, bool> where = null;
                Func<RequirementModel, bool> whereName = model => model.Id > 0;
                if (onlySet != null)
                {
                    if (!onlySet.Value)
                    {
                        where = model => model.Score == null;
                    }
                    else
                    {
                        if (positive == null)
                        {
                            where = model => model.Score != null;
                        }
                        else if (positive.Value)
                        {
                            where = model => model.Score > 0;
                        }
                        else
                        {
                            where = model => model.Score == 0d;
                        }
                    }
                }
                else
                {
                    where = model => model.Id >= 1;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    whereName = model => model.Description.ToLower().Contains(name.ToLower());
                }

                var categories = context.Categories
                    .AsQueryable()
                    .Include(x => x.Requirements).ToList()
                    .OrderBy(x => x.Requirements.First().Id)
                    .Select(x => new CategoryModel
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Number = x.Number,
                        Requirements = x.Requirements.Select(r => new RequirementModel
                        {
                            Id = r.Id,
                            Description = r.Description,
                            PossibleScores = r.PossibleScores.Split(';')
                                .Select(s => Convert.ToDouble(s, CultureInfo.InvariantCulture)).ToArray(),
                            Score = evaluations[r.Id].Score,
                            EvaluationDescription = evaluations[r.Id].Description
                        })
                        .Where(where)
                        .Where(whereName)
                        .ToList()
                    }).ToList()
                    .Where(x => x.Requirements.Any())
                    .ToList();

                return new Page<CategoryModel>
                {
                    Items = categories.Skip(skip).Take(take).ToList(),
                    Total = categories.Count
                };
            }
        }

        public async Task SetEvaluation(Guid inspectionId, int requirementId, double? score, string description)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = context.Inspections
                    .AsQueryable()
                    .Include(x => x.Evaluations)
                    .First(x => x.Id == inspectionId);

                var eval = inspection.Evaluations
                    .First(x => x.RequirementId == requirementId);

                eval.Score = score;
                eval.Description = description;

                if (inspection.Evaluations.All(x => x.Score != null))
                {
                    inspection.Status = InspectionStatus.InReview;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}