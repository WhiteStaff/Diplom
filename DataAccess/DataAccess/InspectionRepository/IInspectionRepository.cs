using System;
using System.Threading.Tasks;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public interface IInspectionRepository
    {
        Task<InspectionModel> CreateInspection(InspectionModel model);

        Task AddInspectionDocument(Guid inspectionId, string documentName, byte[] document);

        Task<DocumentModel> GetInspectionDocument(Guid documentId);
    }
}