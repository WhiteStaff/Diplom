using System;

namespace Common.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}