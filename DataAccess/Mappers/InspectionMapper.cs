using System.Linq;
using DataAccess.DbModels;
using Models;

namespace DataAccess.Mappers
{
    public static class InspectionMapper
    {
        public static InspectionModel Map(this Inspection model)
        {
            return new InspectionModel
            {
                Id = model.Id,
                CompanyId = model.CustomerId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                FinalScore = model.FinalScore,
                Assessors = model.Assessors.Select(x => x.Map()).ToList()
            };
        }

        public static Inspection Map(this InspectionModel model)
        {
            return new Inspection
            {
                Id = model.Id,
                CustomerId = model.CompanyId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                FinalScore = model.FinalScore,
                Assessors = model.Assessors.Select(x => x.Map()).ToList()
            };
        }
    }
}