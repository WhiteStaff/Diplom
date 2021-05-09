using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BizRules.CompanyBizRules;
using Common.Models.Enums;
using Common.Models.RequestModels;
using OAuth;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/company")]
    public class CompanyController : ApiControllerBase
    {
        private readonly ICompanyBizRules _companyBizRules;

        public CompanyController(ICompanyBizRules companyBizRules)
        {
            _companyBizRules = companyBizRules;
        }

        [HttpPost, Route("create")]
        [JwtAuthorize(UserRole.Admin)]
        public async Task<object> CreateCompany(CreateCompanyRequest request)
        {
            try
            {
                return await _companyBizRules.CreateCompany(request);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("list")]
        public async Task<object> GetCompanies(CompanyRole roleFilter)
        {
            try
            {
                return await _companyBizRules.GetCompanies(roleFilter);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<object> DeleteCompany(Guid id)
        {
            try
            {
                await _companyBizRules.DeleteCompany(id);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }
    }
}