using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DbModels
{
    public class Requirement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public string PossibleScores { get; set; }
    }
}