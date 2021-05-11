using System;
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

        public async Task<Page<CompanyModel>> GetCompanies(CompanyRole role, int take, int skip)
        {
            using (var context = new ISControlDbContext())
            {
                var companies = context.Companies.Where(x => x.Role == role).OrderBy(x => x.Name);

                return new Page<CompanyModel>
                {
                    Items = companies.Skip(skip).Take(take).ToList().Select(x => x.ToModel()).ToList(),
                    Total = companies.Count()
                };
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

        public async Task<CompanyModel> GetCompany(Guid companyId)
        {
            using (var context = new ISControlDbContext())
            {
                return (await context.Companies.FirstOrDefaultAsync(x => x.Id == companyId))?.ToModel();
            }
        }

        public async Task<CompanyModel> GetUserCompany(Guid userId)
        {
            using (var context = new ISControlDbContext())
            {
                return (await context.Employees
                    .AsQueryable()
                    .Include(x => x.Company)
                    .FirstAsync(x => x.Id == userId)).Company.ToModel();
            }
        }
    }
}