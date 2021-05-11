using System;
using System.Threading.Tasks;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public interface IInspectionRepository
    {
        Task<InspectionModel> CreateInspection(Guid contractorId, Guid customerId);

        Task AddInspectionDocument(Guid inspectionId, string documentName, byte[] document);

        Task<DocumentModel> GetInspectionDocument(Guid documentId);

        Task<Page<BriefDocumentModel>> GetDocumentList(Guid inspectionId, int take, int skip);

        Task DeleteDocument(Guid documentId);
    }
}