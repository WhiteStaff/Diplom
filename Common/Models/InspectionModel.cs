using System;
using System.Collections.Generic;
using Common.Models;
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

        public InspectionStatus Status { get; set; }

        public double? FinalDigitScore { get; set; }

        public List<UserModel> Assessors { get; set; }

        public List<EventModel> Schedule { get; set; }

        public List<DocumentModel> Documents { get; set; }
    }
}