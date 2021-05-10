using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Enums;
using Models;

namespace DataAccess.DataAccess.CompanyRepository
{
    public interface ICompanyRepository
    {
        Task<CompanyModel> CreateCompany(CompanyModel model);

        Task<Page<CompanyModel>> GetCompanies(CompanyRole role, int take, int skip);

        Task DeleteCompany(Guid id);

        Task<CompanyModel> GetCompany(Guid companyId);
    }
}