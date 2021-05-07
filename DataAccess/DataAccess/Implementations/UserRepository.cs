using System;
using System.Linq;
using DataAccess.DataAccess.Interfaces;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        public UserModel GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(string email, string password)
        {
            using (var context = new ISControlDbContext())
            {
                return context.Employee.FirstOrDefault(x => x.Email == email && x.Password == password)?.Map();
            }
        }
    }
}