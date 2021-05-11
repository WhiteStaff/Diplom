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
    public class InspectionController : ApiControllerBase
    {
        private readonly IInspectionBizRules _inspectionBizRules;

        public InspectionController(IInspectionBizRules inspectionBizRules)
        {
            _inspectionBizRules = inspectionBizRules;
        }

        [HttpPost, Route("create")]
        [JwtAuthorize(UserRole.CompanyAdmin, UserRole.User)]
        public async Task<object> CreateInspection([FromBody]Guid contractorId)
        {
            try
            {
                var userId = new Guid((HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);

                return await _inspectionBizRules.CreateInspection(contractorId,  userId);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPost, Route("documents/create")]
        public async Task<object> AdInspectionDocument(CreateInspectionDocumentRequest request)
        {
            try
            {
                await _inspectionBizRules.AddInspectionDocument(request);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("documents/{documentId}")]
        public async Task<object> GetInspectionDocument([FromUri] Guid documentId)
        {
            try
            {
                var document = await _inspectionBizRules.GetInspectionDocument(documentId);
                return FileContent(document.Name, document.Data);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("documents/list")]
        public async Task<object> GetInspectionDocumentList([FromUri] Guid inspectionId, [FromUri] int take, [FromUri] int skip)
        {
            try
            {
                return await _inspectionBizRules.GetInspectionDocuments(inspectionId, take, skip);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpDelete, Route("documents/{documentId}")]
        public async Task<object> GetInspectionDocumentList(Guid documentId)
        {
            try
            {
                await _inspectionBizRules.DeleteDocument(documentId);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }
    }
}