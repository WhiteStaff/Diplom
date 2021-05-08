using System.Threading.Tasks;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public interface IInspectionRepository
    {
        Task<InspectionModel> CreateInspection(InspectionModel model);
    }
}