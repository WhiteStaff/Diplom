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

        public async Task<List<CompanyModel>> GetCompanies(CompanyRole role)
        {
            return await _companyRepository.GetCompanies(role);
        }

        public async Task DeleteCompany(Guid id)
        {
            await _companyRepository.DeleteCompany(id);
        }
    }
}