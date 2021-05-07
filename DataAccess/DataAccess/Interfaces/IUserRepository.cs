using System;
using Models;

namespace DataAccess.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        UserModel GetUser(Guid id);

        UserModel GetUser(string email, string password);
    }
}