using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Enums;
using Common.Models.Mappers;
using Common.Models.RequestModels;
using DataAccess.DataAccess.CompanyRepository;
using Models;

namespace BizRules.CompanyBizRules
{
    public class CompanyBizRules : ICompanyBizRules
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyBizRules(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyModel> CreateCompany(CreateCompanyRequest request)
        {
            return await _companyRepository.CreateCompany(request.ToModel());
        }

        public async Task<Page<CompanyModel>> GetCompanies(CompanyRole role, int take, int skip)
        {
            return await _companyRepository.GetCompanies(role, take, skip);
        }

        public async Task DeleteCompany(Guid id)
        {
            await _companyRepository.DeleteCompany(id);
        }

        public async Task<CompanyModel> GetCompany(Guid companyId)
        {
            return await _companyRepository.GetCompany(companyId);
        }
    }
}