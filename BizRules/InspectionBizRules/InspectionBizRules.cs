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

        public async Task<InspectionModel> CreateInspection(CreateInspectionRequest request, Guid userId)
        {
            var user = _userRepository.GetUser(userId);
            var company = await _companyRepository.GetCompany(request.CustomerId);

            if (company == null)
            {
                throw new Exception("Company not found.");
            }

            if (company.Role != CompanyRole.Customer)
            {
                throw new Exception("Company must be Customer.");
            }

            var inspectionModel = request.Map();
            var users = await _userRepository.GetCompanyUsers(user.CompanyId.Value);
            inspectionModel.Assessors = users.Where(x => request.AssessorIds.Contains(x.Id)).ToList();

            if (!inspectionModel.Assessors.Any())
            {
                throw new Exception("Inspection should have assessors.");
            }

            return await _inspectionRepository.CreateInspection(inspectionModel);
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