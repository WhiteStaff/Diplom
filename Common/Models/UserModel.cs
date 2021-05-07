using System;

namespace Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public Guid? CompanyId { get; set; }

        public string Email { get; set; }
    }
}