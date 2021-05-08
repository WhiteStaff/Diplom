using System.Linq;
using System.Threading.Tasks;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public class InspectionRepository : IInspectionRepository
    {
        public async Task<InspectionModel> CreateInspection(InspectionModel model)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = model.Map();
                var assessorIds = inspection.Assessors.Select(x => x.Id);
                inspection.Assessors = context.Employees.Where(x => assessorIds.Contains(x.Id)).ToList();
                
                context.Inspections.Add(inspection);

                await context.SaveChangesAsync();

                return model;
            }
        }
    }
}