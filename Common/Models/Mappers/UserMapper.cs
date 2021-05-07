using System;
using Common.Models.RequestModels;
using Models;

namespace Common.Models.Mappers
{
    public static class UserMapper
    {
        public static UserFullModel ToFullModel(this CreateUserRequest model)
        {
            return new UserFullModel
            {
                Id = new Guid(),
                CompanyId = model.CompanyId,
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
                UserRole = model.UserRole
            };
        }

        public static UserModel ToBriefModel(this UserFullModel model)
        {
            return new UserModel
            {
                Id = model.Id,
                CompanyId = model.CompanyId,
                Email = model.Email,
                Name = model.Name,
                UserRole = model.UserRole
            };
        }
    }
}