using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Models;

namespace DataAccess.DataAccess.EvaluationRepository
{
    public class EvaluationRepository : IEvaluationRepository
    {
        public async Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId,
            int take,
            int skip,
            bool? onlySet,
            bool? positive)
        {
            using (var context = new ISControlDbContext())
            {
                var scores = context.Inspections
                    .AsQueryable()
                    .Include(x => x.Evaluations)
                    .First(x => x.Id == inspectionId)
                    .Evaluations
                    .ToDictionary(x => x.RequirementId, x => x.Score);

                Func<RequirementModel, bool> where = null;
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
                                .Select(s => Convert.ToDouble(s, CultureInfo.InvariantCulture)).ToArray()
                        }).Where(where).ToList()
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
                var eval = context.Inspections
                    .AsQueryable()
                    .Include(x => x.Evaluations)
                    .First(x => x.Id == inspectionId).Evaluations
                    .First(x => x.RequirementId == requirementId);

                eval.Score = score;
                eval.Description = description;
                await context.SaveChangesAsync();
            }
        }
    }
}