﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Enums;

namespace DataAccess.DbModels
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CompanyRole Role { get; set; }

        public byte[] Image { get; set; }

        public IList<Employee> Employees { get; set; }

        public IList<Inspection> Inspections { get; set; }
    }
}