using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BizRules.InspectionBizRules;
using Common.Models.Enums;
using Common.Models.RequestModels;
using Models;
using OAuth;

namespace Diplom.Controllers
{
    [Authorize]
    [RoutePrefix("api/inspection")]
    public class InspectionController : ApiController
    {
        private readonly IInspectionBizRules _inspectionBizRules;

        public InspectionController(IInspectionBizRules inspectionBizRules)
        {
            _inspectionBizRules = inspectionBizRules;
        }

        [HttpPost, Route("create")]
        [JwtAuthorize(UserRole.CompanyAdmin)]
        public async Task<object> CreateInspection(CreateInspectionRequest request)
        {
            try
            {
                var userId = new Guid((HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);

                return await _inspectionBizRules.CreateInspection(request,  userId);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }
    }
}