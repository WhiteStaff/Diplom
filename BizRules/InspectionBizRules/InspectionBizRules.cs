using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Enums;
using Common.Models.Mappers;
using Common.Models.RequestModels;
using DataAccess.DataAccess.CompanyRepository;
using DataAccess.DataAccess.InspectionRepository;
using DataAccess.DataAccess.UserRepository;
using Models;

namespace BizRules.InspectionBizRules
{
    public class InspectionBizRules : IInspectionBizRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly ICompanyRepository _companyRepository;

        public InspectionBizRules(IUserRepository userRepository, IInspectionRepository inspectionRepository, ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _inspectionRepository = inspectionRepository;
            _companyRepository = companyRepository;
        }

        public async Task<InspectionModel> CreateInspection(Guid contractorId, Guid userId)
        {
            var user = _userRepository.GetUser(userId);
            var customer = await _companyRepository.GetCompany(user.CompanyId.Value);
            var contractor = await _companyRepository.GetCompany(contractorId);

            if (customer == null)
            {
                throw new Exception("Company not found.");
            }

            if (customer.Role != CompanyRole.Customer)
            {
                throw new Exception("Company must be Customer.");
            }

            if (contractor.Role != CompanyRole.Contractor)
            {
                throw new Exception("Contractor company must be Contractor.");
            }

            return await _inspectionRepository.CreateInspection(contractorId, customer.Id);
        }

        public async Task AddInspectionDocument(CreateInspectionDocumentRequest request)
        {
            await _inspectionRepository.AddInspectionDocument(request.InspectionId, request.DocumentName,
                request.DocumentData);
        }

        public async Task<DocumentModel> GetInspectionDocument(Guid documentId)
        {
            return await _inspectionRepository.GetInspectionDocument(documentId);
        }

        public async Task<Page<BriefDocumentModel>> GetInspectionDocuments(Guid inspectionID, int take, int skip)
        {
            return await _inspectionRepository.GetDocumentList(inspectionID, take, skip);
        }

        public async Task DeleteDocument(Guid documentId)
        {
            await _inspectionRepository.DeleteDocument(documentId);
        }
    }
}