using Common.Models;
using DataAccess.DbModels;

namespace DataAccess.Mappers
{
    public static class CategoriesMapper
    {
        public static CategoryModel Map(this Category category)
        {
            return new CategoryModel
            {
                Id = category.Id,
                Description = category.Description,
                Number = category.Number
            };
        }
    }
}