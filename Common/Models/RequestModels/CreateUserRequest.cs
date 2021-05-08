using System;
using System.ComponentModel.DataAnnotations;
using Common.Models.Enums;

namespace Common.Models.RequestModels
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid? CompanyId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}