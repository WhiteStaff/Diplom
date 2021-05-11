using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Models.Enums;
using Common.Models.Mappers;
using Common.Models.RequestModels;
using DataAccess.DataAccess.CompanyRepository;
using DataAccess.DataAccess.InspectionRepository;
using DataAccess.DataAccess.UserRepository;
using Models;

namespace BizRules.UsersBizRules
{
    public class UsersBizRules : IUsersBizRules
    {
        private readonly IUserRepository _userRepository;
        private readonly IInspectionRepository _inspectionRepository;
        private readonly ICompanyRepository _companyRepository;

        public UsersBizRules(
            IUserRepository userRepository,
            IInspectionRepository inspectionRepository, 
            ICompanyRepository companyRepository)
        {
            _userRepository = userRepository;
            _inspectionRepository = inspectionRepository;
            _companyRepository = companyRepository;
        }

        public async Task<UserModel> CreateUser(CreateUserRequest request)
        {
            request.Password = request.Password.HashPassword();
            if (request.CompanyId == null)
            {
                throw new Exception("User should have company.");
            }

            return (await _userRepository.CreateUser(request.ToFullModel())).ToBriefModel();
        }

        public async Task<UserModel> CreateCompanyAdminUser(CreateUserRequest request)
        {
            request.Password = request.Password.HashPassword();
            if (request.CompanyId == null)
            {
                throw new Exception("User should have company.");
            }

            return (await _userRepository.CreateCompanyAdminUser(request.ToFullModel())).ToBriefModel();
        }

        public async Task<UserModel> CreateAdminUser(CreateUserRequest request)
        {
            request.Password = request.Password.HashPassword();
            request.CompanyId = null;
            return (await _userRepository.CreateAdminUser(request.ToFullModel())).ToBriefModel();
        }

        public async Task<UserModel> GetUser(Guid id)
        {
            return _userRepository.GetUser(id);
        }

        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteUser(id);
        }

        public async Task<List<UserModel>> GetCompanyUsers(Guid companyId)
        {
            return await _userRepository.GetCompanyUsers(companyId);
        }

        public async Task<Page<InspectionModel>> GetMyInspections(Guid userId, int take, int skip)
        {
            var company = await _companyRepository.GetUserCompany(userId);

            if (company.Role == CompanyRole.Contractor)
            {
               return await _inspectionRepository.GetCompanyActiveInspections(company.Id, take, skip);
            }
            else
            {
                return await _inspectionRepository.GetCompanyArchiveInspections(company.Id, take, skip);
            }
        }
    }
}