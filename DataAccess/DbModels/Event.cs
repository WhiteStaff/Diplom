using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Event
    {
        public Guid Id { get; set; }

        [ForeignKey("Inspection")]
        public Guid InspectionId { get; set; }

        public Inspection Inspection { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}