using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Evaluation
    {
        public Guid Id { get; set; }

        [ForeignKey("Inspection")]
        public Guid InspectionId { get; set; }

        public Inspection Inspection { get; set; }

        [ForeignKey("Requirement")]
        public Guid RequirementId { get; set; }

        public Requirement Requirement { get; set; }
    }
}