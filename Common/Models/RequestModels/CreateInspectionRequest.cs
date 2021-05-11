using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models.Attributes;

namespace Common.Models.RequestModels
{
    public class CreateInspectionRequest
    {
        [Required]
        public Guid CustomerId { get; set; }

        [RequireNonDefault]
        public DateTime StartDate { get; set; }

        [RequireNonDefault]
        public IList<Guid> AssessorIds { get; set; }
    }
}