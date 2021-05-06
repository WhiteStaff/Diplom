using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Document
    {
        public Guid Id { get; set; }

        [ForeignKey("Inspection")]
        public Guid InspectionId { get; set; }

        public Guid Inspection { get; set; }

        public byte[] Data { get; set; }
    }
}