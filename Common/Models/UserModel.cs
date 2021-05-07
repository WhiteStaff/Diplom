using System;
using Common.Models.Enums;

namespace Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? CompanyId { get; set; }

        public string Email { get; set; }

        public UserRole UserRole { get; set; }
    }
}