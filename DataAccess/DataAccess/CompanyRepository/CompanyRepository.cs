using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Enums;
using DataAccess.DbModels;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.CompanyRepository
{
    public class CompanyRepository : ICompanyRepository
    {
        public async Task<CompanyModel> CreateCompany(CompanyModel model)
        {
            using (var context = new ISControlDbContext())
            {
                var isCompanyExists = context.Companies.FirstOrDefault(x => x.Name == model.Name) != null;
                if (isCompanyExists)
                {
                    throw new Exception("Company with same name already exists.");
                }
                
                model.Id = Guid.NewGuid();
                var company = new Company
                {
                    Id = model.Id,
                    Description = model.Description,
                    Image = model.Image,
                    Name = model.Name,
                    Role = model.Role
                };

                context.Companies.Add(company);
                await context.SaveChangesAsync();

                return model;

            }
        }

        public async Task<List<CompanyModel>> GetCompanies(CompanyRole role)
        {
            using (var context = new ISControlDbContext())
            {
                return context.Companies.Where(x => x.Role == role).ToList().Select(x => x.ToModel()).ToList();
            }
        }

        public async Task DeleteCompany(Guid id)
        {
            using (var context = new ISControlDbContext())
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == id);
                if (company == null)
                {
                    throw new Exception("Company not found");
                }

                context.Companies.Remove(company);
                await context.SaveChangesAsync();
            }
        }
    }
}