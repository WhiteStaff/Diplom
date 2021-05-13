using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BizRules.InspectionBizRules;
using Common.Models;
using Common.Models.Enums;
using Common.Models.RequestModels;
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
        public async Task<object> CreateInspection([FromUri]Guid contractorId)
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

        [HttpPost, Route("start/{inspectionId}")]
        [JwtAuthorize(UserRole.CompanyAdmin, UserRole.User)]
        public async Task<object> StartInspection(Guid inspectionId, [FromBody] StartInspectionRequest request)
        {
            try
            {
                return await _inspectionBizRules.StartInspection(inspectionId, request.AssessorIds);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("{inspectionId}")]
        public async Task<object> GetInspection(Guid inspectionId)
        {
            try
            {
                return await _inspectionBizRules.GetInspection(inspectionId);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPost, Route("{inspectionId}/addEvent")]
        public async Task<object> AddInspectionEvent(Guid inspectionId, [FromBody] EventModel model)
        {
            try
            {
                return await _inspectionBizRules.AddEvent(inspectionId, model);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpDelete, Route("{inspectionId}/deleteEvent")]
        public async Task<object> GetInspection(Guid inspectionId, [FromUri] Guid eventId)
        {
            try
            {
                return await _inspectionBizRules.DeleteEvent(inspectionId, eventId);
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

        [HttpPost, Route("{inspectionId}/evaluations")]
        public async Task<object> GetInspectionEvaluations(Guid inspectionId, [FromBody] GetEvaluationRequest request)
        {
            try
            {
                return await _inspectionBizRules.GetEvaluations(inspectionId, request.Take, request.Skip, request.OnlySet, request.Positive, request.Name);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPut, Route("{inspectionId}/evaluations")]
        public async Task<object> SetEvaluation(Guid inspectionId, [FromBody] SetEvaluationRequest request)
        {
            try
            {
                await _inspectionBizRules.SetEvaluation(inspectionId, request.RequirementId, request.Score, request.Description);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPut, Route("{inspectionId}/updateStatus")]
        [JwtAuthorize(UserRole.CompanyAdmin)]
        public async Task<object> UpdateStatus(Guid inspectionId, [FromBody] UpdateInspectionStatusRequest request)
        {
            try
            {
                await _inspectionBizRules.UpdateInspectionStatus(inspectionId, request.Status);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpPut, Route("{inspectionId}/approve")]
        [JwtAuthorize(UserRole.CompanyAdmin)]
        public async Task<object> Approve(Guid inspectionId)
        {
            try
            {
                var userId = new Guid((HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);

                await _inspectionBizRules.ApproveInspection(userId, inspectionId);
                return Ok();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("{inspectionId}/genericDocument/first")]
        public async Task<object> GetFirstDocument(Guid inspectionId)
        {
            try
            {
                var file = await _inspectionBizRules.GenerateFirstForm(inspectionId);
                var fileName = await _inspectionBizRules.ResolveFormName(inspectionId, "1");
                return FileContent(fileName, file);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("{inspectionId}/genericDocument/second")]
        public async Task<object> GetSecondDocument(Guid inspectionId)
        {
            try
            {
                var file = await _inspectionBizRules.GenerateSecondForm(inspectionId);
                var fileName = await _inspectionBizRules.ResolveFormName(inspectionId, "2");
                return FileContent(fileName, file);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }

        [HttpGet, Route("last")]
        public async Task<object> GetLastInspection()
        {
            try
            {
                var userId = new Guid((HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value);

                return await _inspectionBizRules.GetLastOrderedInspection(userId);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, e.Message);
            }
        }
    }
}