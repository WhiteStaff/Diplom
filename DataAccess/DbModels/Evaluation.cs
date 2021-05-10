using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Evaluation
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Inspection")]
        public Guid InspectionId { get; set; }

        public Inspection Inspection { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Requirement")]
        public int RequirementId { get; set; }

        public Requirement Requirement { get; set; }

        public double? Score { get; set; }
    }
}