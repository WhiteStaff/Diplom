using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.RequestModels;
using Models;

namespace BizRules.UsersBizRules
{
    public interface IUsersBizRules
    {
        Task<UserModel> CreateUser(CreateUserRequest request);

        Task<UserModel> GetUser(Guid id);

        Task DeleteUser(Guid id);

        Task<List<UserModel>> GetCompanyUsers(Guid companyId);
    }
}