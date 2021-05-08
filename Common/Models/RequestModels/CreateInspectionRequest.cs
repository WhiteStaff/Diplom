using System;
using System.Collections.Generic;

namespace Common.Models.RequestModels
{
    public class CreateInspectionRequest
    {
        public Guid CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public List<Guid> AssessorIds { get; set; }
    }
}