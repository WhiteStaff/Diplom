using DataAccess.DbModels;
using Models;

namespace DataAccess.Mappers
{
    public static class DocumentMapper
    {
        public static DocumentModel Map(this Document model)
        {
            return new DocumentModel
            {
                Id = model.Id,
                Name = model.Name,
                Data = model.Data
            };
        }
    }
}