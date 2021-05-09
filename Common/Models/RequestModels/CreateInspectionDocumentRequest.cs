using System;

namespace Common.Models.RequestModels
{
    public class CreateInspectionDocumentRequest
    {
        public Guid InspectionId { get; set; }

        public string DocumentName { get; set; }

        public byte[] DocumentData { get; set; }
    }
}