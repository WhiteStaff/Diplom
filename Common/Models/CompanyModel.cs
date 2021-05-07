using System;
using DataAccess.Enums;

namespace Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CompanyRole Role { get; set; }

        public byte[] Image { get; set; }
    }
}