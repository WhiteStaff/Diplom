using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Enums;
using Common.Models.RequestModels;
using Models;

namespace BizRules.InspectionBizRules
{
    public interface IInspectionBizRules
    {
        Task<InspectionModel> CreateInspection(Guid contractorId, Guid userId);

        Task<InspectionModel> StartInspection(Guid inspectionId, List<Guid> assessorIds);

        Task<InspectionModel> GetInspection(Guid inspectionId);

        Task<List<EventModel>> AddEvent(Guid inspectionId, EventModel model);

        Task<List<EventModel>> DeleteEvent(Guid inspectionId, Guid eventId);

        Task AddInspectionDocument(CreateInspectionDocumentRequest request);

        Task<DocumentModel> GetInspectionDocument(Guid documentId);

        Task<Page<BriefDocumentModel>> GetInspectionDocuments(Guid inspectionId, int take, int skip);

        Task DeleteDocument(Guid documentId);

        Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId, int take, int skip, bool? onlySet, bool? positive, string name);

        Task SetEvaluation(Guid inspectionId, int reqId, double? score, string description);

        Task UpdateInspectionStatus(Guid inspectionId, InspectionStatus status);

        Task ApproveInspection(Guid userId, Guid inspectionId);

        Task<byte[]> GenerateFirstForm(Guid inspectionId);

        Task<byte[]> GenerateSecondForm(Guid inspectionId);

        Task<string> ResolveFormName(Guid inspectionId, string formNumber);

        Task<InspectionModel> GetLastOrderedInspection(Guid userId);
    }
}