using System;
using System.Collections.Generic;
using Common.Models.Attributes;

namespace Common.Models.RequestModels
{
    public class StartInspectionRequest
    {
        [RequireNonDefault]
        public List<Guid> AssessorIds { get; set; }
    }
}