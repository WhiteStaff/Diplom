using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class CategoryModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        public List<RequirementModel> Requirements { get; set; }
    }
}