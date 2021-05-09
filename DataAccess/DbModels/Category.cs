using System;
using System.Collections.Generic;

namespace DataAccess.DbModels
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public IList<Requirement> Requirements { get; set; }
    }
}