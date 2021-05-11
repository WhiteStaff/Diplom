using System;

namespace Common.Models.RequestModels
{
    public class SetEvaluationRequest
    {
        public int RequirementId { get; set; }

        public double? Score { get; set; }

        public string Description { get; set; }
    }
}