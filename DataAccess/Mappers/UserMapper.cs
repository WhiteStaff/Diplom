using DataAccess.DbModels;
using Models;

namespace DataAccess.Mappers
{
    public static class UserMapper
    {
        public static UserModel Map(this Employee user)
        {
            return new UserModel
            {
                Id = user.Id,
                CompanyId = user.CompanyId,
                Email = user.Email
            };
        }
    }
}