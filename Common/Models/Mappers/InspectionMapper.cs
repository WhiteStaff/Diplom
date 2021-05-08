using System;
using Common.Models.RequestModels;
using Models;

namespace Common.Models.Mappers
{
    public static class InspectionMapper
    {
        public static InspectionModel Map(this CreateInspectionRequest model)
        {
            return new InspectionModel
            {
                Id = Guid.NewGuid(),
                CompanyId = model.CustomerId,
                StartDate = model.StartDate
            };
        }
    }
}