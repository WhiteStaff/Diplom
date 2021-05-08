using System;
using System.Collections.Generic;
using Common.Models.Enums;

namespace Models
{
    public class InspectionModel
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Score? FinalScore { get; set; }

        public List<UserModel> Assessors { get; set; }
    }
}