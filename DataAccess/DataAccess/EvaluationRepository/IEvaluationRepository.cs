using System;
using System.Threading.Tasks;
using Common.Models;
using Models;

namespace DataAccess.DataAccess.EvaluationRepository
{
    public interface IEvaluationRepository
    {
        Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId, int take, int skip, bool? onlySet, bool? positive);

        Task SetEvaluation(Guid inspectionId, int requirementId, double? score);
    }
}