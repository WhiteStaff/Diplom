using System;
using DataAccess.DbModels;
using Models;

namespace DataAccess.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyModel ToModel(this Company model)
        {
            return new CompanyModel
            {
                Id = model.Id,
                Description = model.Description,
                Image = model.Image,
                Name = model.Name,
                Role = model.Role
            };
        }
    }
}