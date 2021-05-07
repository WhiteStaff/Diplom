using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BizRules.CompanyBizRules;
using Common.Models.Enums;
using Common.Models.RequestModels;
using Models;
using OAuth;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyBizRules _companyBizRules;

        public CompanyController(ICompanyBizRules companyBizRules)
        {
            _companyBizRules = companyBizRules;
        }

        [HttpPost, Route("create")]
        [JwtAuthorize(UserRole.Admin)]
        public async Task<CompanyModel> CreateCompany(CreateCompanyRequest request)
        {
            return await _companyBizRules.CreateCompany(request);
        }

        [HttpGet, Route("list")]
        public async Task<List<CompanyModel>> GetCompanies(CompanyRole roleFilter)
        {
            return await _companyBizRules.GetCompanies(roleFilter);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> DeleteCompany(Guid id)
        {
            await _companyBizRules.DeleteCompany(id);
            return Ok();
        }
    }
}