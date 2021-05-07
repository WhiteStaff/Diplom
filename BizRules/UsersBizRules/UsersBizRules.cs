﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Models.Mappers;
using Common.Models.RequestModels;
using DataAccess.DataAccess.UserRepository;
using Models;

namespace BizRules.UsersBizRules
{
    public class UsersBizRules : IUsersBizRules
    {
        private readonly IUserRepository _userRepository;

        public UsersBizRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserModel> CreateUser(CreateUserRequest request)
        {
            request.Password = request.Password.HashPassword();
            return (await _userRepository.CreateUser(request.ToFullModel())).ToBriefModel();
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
    }
}