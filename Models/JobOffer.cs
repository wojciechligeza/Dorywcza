using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dorywcza.Models
{
    public class JobOffer
    {
        public int JobOfferId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }
        public string TimeFrame { get; set; }
        public int AmountOfPlaces { get; set; }
        public DateTime AddDate { get; set; }
        public bool QualificationIsRequired { get; set; }
        public bool State { get; set; }

        public virtual Category category { get; set; }
        public virtual Employee employee { get; set; }
        public virtual Employer employer { get; set; }
    }
}
