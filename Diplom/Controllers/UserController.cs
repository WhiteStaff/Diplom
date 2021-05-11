using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BizRules.UsersBizRules;
using Common.Models.Enums;
using Common.Models.RequestModels;
using OAuth;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiControllerBase
    {
        private readonly IUsersBizRules _usersBizRules;

        public UserController(IUsersBizRules usersBizRules)
        {
            _usersBizRules = usersBizRules;
        }

        [HttpPost, Route("createUser")]
        [JwtAuthorize(UserRole.Admin, UserRole.CompanyAdmin)]
        public async Task<object> CreateUser(CreateUserRequest request)
        {
            try
            {
                return await _usersBizRules.CreateUser(request);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPost, Route("createCompanyAdmin")]
        [JwtAuthorize(UserRole.Admin, UserRole.CompanyAdmin)]
        public async Task<object> CreateCompanyAdminUser(CreateUserRequest request)
        {
            try
            {
                return await _usersBizRules.CreateCompanyAdminUser(request);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPost, Route("createAdmin")]
        [JwtAuthorize(UserRole.Admin)]
        public async Task<object> CreateAdminUser(CreateUserRequest request)
        {
            try
            {
                return await _usersBizRules.CreateAdminUser(request);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<object> GetUser(Guid id)
        {
            try
            {
                return await _usersBizRules.GetUser(id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<object> Delete(Guid id)
        {
            try
            {
                await _usersBizRules.DeleteUser(id);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("list")]
        public async Task<object> GetCompanyEmployees(Guid companyId)
        {
            try
            {
                return await _usersBizRules.GetCompanyUsers(companyId);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("inspections")]
        [JwtAuthorize(UserRole.CompanyAdmin, UserRole.User)]
        public async Task<object> GetMyInspections([FromUri] int take, [FromUri] int skip)
        {
            try
            {
                var userId = new Guid((HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);
                return await _usersBizRules.GetMyInspections(userId, take, skip);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }
    }
}