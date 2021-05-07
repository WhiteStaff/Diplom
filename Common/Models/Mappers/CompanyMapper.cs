using System;
using Common.Models.RequestModels;
using Models;

namespace Common.Models.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyModel ToModel(this CreateCompanyRequest model)
        {
            return new CompanyModel
            {
                Id = new Guid(),
                Description = model.Description,
                Image = model.Image,
                Name = model.Name,
                Role = model.Role
            };
        }


    }
}