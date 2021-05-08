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
                Name = user.Name,
                CompanyId = user.CompanyId,
                Email = user.Email,
                UserRole = user.Role
            };
        }

        public static Employee Map(this UserModel user)
        {
            return new Employee
            {
                Id = user.Id,
                Name = user.Name,
                CompanyId = user.CompanyId,
                Email = user.Email,
                Role = user.UserRole
            };
        }
    }
}