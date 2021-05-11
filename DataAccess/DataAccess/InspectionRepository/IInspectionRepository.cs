using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public interface IInspectionRepository
    {
        Task<InspectionModel> CreateInspection(Guid contractorId, Guid customerId);

        Task<InspectionModel> StartInspection(Guid inspectionId, List<Guid> assessorIds);

        Task<InspectionModel> GetInspection(Guid inspectionId);

        Task<List<EventModel>> AddInspectionEvent(Guid inspectionId, EventModel eventModel);

        Task<List<EventModel>> DeleteInspectionEvent(Guid inspectionId, Guid eventId);

        Task AddInspectionDocument(Guid inspectionId, string documentName, byte[] document);

        Task<DocumentModel> GetInspectionDocument(Guid documentId);

        Task<Page<BriefDocumentModel>> GetDocumentList(Guid inspectionId, int take, int skip);

        Task DeleteDocument(Guid documentId);

        Task<Page<InspectionModel>> GetCompanyArchiveInspections(Guid companyId, int take, int skip);

        Task<Page<InspectionModel>> GetCompanyActiveInspections(Guid companyId, int take, int skip);
    }
}