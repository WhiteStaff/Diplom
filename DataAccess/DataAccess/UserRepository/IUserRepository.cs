using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DataAccess.DataAccess.UserRepository
{
    public interface IUserRepository
    {
        UserModel GetUser(Guid id);

        UserModel GetUser(string email, string password);

        Task<UserFullModel> CreateUser(UserFullModel model);

        Task DeleteUser(Guid id);

        Task<List<UserModel>> GetCompanyUsers(Guid companyId);
    }
}