using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;

namespace DataAccess.DbModels
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Company")]
        public Guid? CompanyId { get; set; }

        public Company Company { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IList<Inspection> Inspections { get; set; }

        public UserRole Role { get; set; }
    }
}