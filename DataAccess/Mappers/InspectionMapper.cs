using System;
using System.Linq;
using Common.Models;
using DataAccess.DbModels;
using Models;

namespace DataAccess.Mappers
{
    public static class InspectionMapper
    {
        public static InspectionModel Map(this Inspection model)
        {
            return new InspectionModel
            {
                Id = model.Id,
                CompanyId = model.CustomerId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                FinalScore = model.FinalScore,
                Status = model.Status,
                Assessors = model.Assessors?.Select(x => x.Employee)?.ToList()?.Select(x => x.Map())?.ToList(),
                Schedule = model.Schedule?.OrderBy(x => x.Date)?.ToList()?.Select(x => x.Map())?.ToList(),
                Documents = model.Documents?.OrderBy(x => x.Name)?.Select(x => x.Map())?.ToList()
            };
        }

        public static EventModel Map(this Event model)
        {
            return new EventModel
            {
                Id = model.Id,
                Date = model.Date,
                Description = model.Description
            };
        }

        public static Event Map(this EventModel model)
        {
            return new Event
            {
                Id = model.Id == default ? Guid.NewGuid() : model.Id,
                Date = model.Date,
                Description = model.Description
            };
        }
    }
}