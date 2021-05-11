using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Enums;
using Common.Models.RequestModels;
using DataAccess.DataAccess.CompanyRepository;
using DataAccess.DataAccess.EvaluationRepository;
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
        private readonly IEvaluationRepository _evaluationRepository;

        public InspectionBizRules(
            IUserRepository userRepository, 
            IInspectionRepository inspectionRepository, 
            ICompanyRepository companyRepository, 
            IEvaluationRepository evaluationRepository)
        {
            _userRepository = userRepository;
            _inspectionRepository = inspectionRepository;
            _companyRepository = companyRepository;
            _evaluationRepository = evaluationRepository;
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

        public async Task<InspectionModel> StartInspection(Guid inspectionId, List<Guid> assessorIds)
        {
            return await _inspectionRepository.StartInspection(inspectionId, assessorIds);
        }

        public async Task<InspectionModel> GetInspection(Guid inspectionId)
        {
            return await _inspectionRepository.GetInspection(inspectionId);
        }

        public async Task<List<EventModel>> AddEvent(Guid inspectionId, EventModel model)
        {
            return await _inspectionRepository.AddInspectionEvent(inspectionId, model);
        }

        public async Task<List<EventModel>> DeleteEvent(Guid inspectionId, Guid eventId)
        {
            return await _inspectionRepository.DeleteInspectionEvent(inspectionId, eventId);
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

        public async Task<Page<CategoryModel>> GetEvaluations(Guid inspectionId, int take, int skip, bool? onlySet, bool? positive)
        {
            return await _evaluationRepository.GetEvaluations(inspectionId, take, skip, onlySet, positive);
        }

        public async Task SetEvaluation(Guid inspectionId, int reqId, double? score, string description) 
        {
            var inspection = await GetInspection(inspectionId);
            if (inspection.Status != InspectionStatus.InProgress)
            {
                throw new Exception("Wrong status to set evaluations.");
            }

            await _evaluationRepository.SetEvaluation(inspectionId, reqId, score, description);
        }

        public async Task UpdateInspectionStatus(Guid inspectionId, InspectionStatus status)
        {
            await _inspectionRepository.UpdateInspectionStatus(inspectionId, status);
        }
    }
}