using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DbModels;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.UserRepository
{
    public class UserRepository : IUserRepository
    {
        public UserModel GetUser(Guid id)
        {
            using (var context = new ISControlDbContext())
            {
                return context.Employees.FirstOrDefault(x => x.Id == id)?.Map();
            }
        }

        public UserModel GetUser(string email, string password)
        {
            using (var context = new ISControlDbContext())
            {
                return context.Employees.FirstOrDefault(x => x.Email == email && x.Password == password)?.Map();
            }
        }

        public async Task<UserFullModel> CreateUser(UserFullModel model)
        {
            using (var context = new ISControlDbContext())
            {
                var userExists = context.Employees.FirstOrDefault(x => x.Email == model.Email) != null;
                if (!userExists)
                {
                    model.Id = Guid.NewGuid();
                    var employee = new Employee
                    {
                        Id = model.Id,
                        Name = model.Name,
                        CompanyId = model.CompanyId,
                        Email = model.Email,
                        Password = model.Password,
                        Role = model.UserRole
                    };
                    context.Employees.Add(employee);
                    await context.SaveChangesAsync();

                    return model;
                }
                else
                {
                    throw new DuplicateNameException($"User with email {model.Email} already exists");
                }
            }
        }

        public async Task DeleteUser(Guid id)
        {
            using (var context = new ISControlDbContext())
            {
                var user = context.Employees.FirstOrDefault(x => x.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                context.Employees.Remove(user);

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UserModel>> GetCompanyUsers(Guid companyId)
        {
            using (var context = new ISControlDbContext())
            {
                return context.Employees.Where(x => x.CompanyId == companyId).ToList().Select(x => x.Map()).ToList();
            }
        }
    }
}