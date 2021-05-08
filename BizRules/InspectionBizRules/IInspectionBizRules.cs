using System;
using System.Threading.Tasks;
using Common.Models.RequestModels;
using Models;

namespace BizRules.InspectionBizRules
{
    public interface IInspectionBizRules
    {
        Task<InspectionModel> CreateInspection(CreateInspectionRequest request, Guid userId);
    }
}