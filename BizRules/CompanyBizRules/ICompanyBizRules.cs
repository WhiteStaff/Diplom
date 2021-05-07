using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Enums;
using Common.Models.RequestModels;
using Models;

namespace BizRules.CompanyBizRules
{
    public interface ICompanyBizRules
    {
        Task<CompanyModel> CreateCompany(CreateCompanyRequest request);

        Task<List<CompanyModel>> GetCompanies(CompanyRole role);

        Task DeleteCompany(Guid id);
    }
}