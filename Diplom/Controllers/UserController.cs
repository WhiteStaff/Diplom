using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BizRules.UsersBizRules;
using Common.Models.Enums;
using Common.Models.RequestModels;
using Models;
using OAuth;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUsersBizRules _usersBizRules;

        public UserController(IUsersBizRules usersBizRules)
        {
            _usersBizRules = usersBizRules;
        }

        [HttpPost, Route("create")]
        [JwtAuthorize(UserRole.Admin)]
        public async Task<UserModel> CreateUser(CreateUserRequest request)
        {
            return await _usersBizRules.CreateUser(request);
        }

        [HttpGet, Route("{id}")]
        public async Task<UserModel> GetUser(Guid id)
        {
            return await _usersBizRules.GetUser(id);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            await _usersBizRules.DeleteUser(id);
            return Ok();
        }

        [HttpGet, Route("list")]
        public async Task<List<UserModel>> GetCompanyEmployees(Guid companyId)
        {
            return await _usersBizRules.GetCompanyUsers(companyId);
        }
    }
}