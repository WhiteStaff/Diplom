using Common.Models.Attributes;

namespace Common.Models.RequestModels
{
    public class GetEvaluationRequest
    {
        [RequireNonDefault]
        public int Take { get; set; }

        [RequireNonDefault]
        public int Skip { get; set; }

        public bool? OnlySet { get; set; }

        public bool? Positive { get; set; }

        public string Name { get; set; }
    }
}