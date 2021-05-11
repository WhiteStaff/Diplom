using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class EmployeeInspection
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Inspection")]
        public Guid InspectionId { get; set; }

        public Inspection Inspection { get; set; }

        public bool Approved { get; set; }
    }
}