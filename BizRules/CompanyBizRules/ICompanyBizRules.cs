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

        Task<Page<CompanyModel>> GetCompanies(CompanyRole role, int take, int skip);

        Task DeleteCompany(Guid id);
    }
}