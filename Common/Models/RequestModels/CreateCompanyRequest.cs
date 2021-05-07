using Common.Models.Enums;

namespace Common.Models.RequestModels
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public CompanyRole Role { get; set; }

        public byte[] Image { get; set; }
    }
}