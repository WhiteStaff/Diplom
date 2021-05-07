using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.Enums;

namespace DataAccess.DbModels
{
    public class Inspection
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        public Company Customer { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Score FinalScore { get; set; }

        public IList<Employee> Assessors { get; set; }

        public IList<Event> Schedule { get; set; }

        public IList<Evaluation> Evaluations { get; set; }
    }
}