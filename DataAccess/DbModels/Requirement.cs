using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Requirement
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}