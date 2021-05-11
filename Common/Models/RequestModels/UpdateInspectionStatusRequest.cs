using Common.Models.Enums;

namespace Common.Models.RequestModels
{
    public class UpdateInspectionStatusRequest
    {
        public InspectionStatus Status { get; set; }
    }
}