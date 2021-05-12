namespace Common.Models
{
    public class RequirementModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double[] PossibleScores { get; set; }

        public double? Score { get; set; }

        public string EvaluationDescription { get; set; }
    }
}