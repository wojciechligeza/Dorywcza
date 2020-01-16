using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dorywcza.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [RegularExpression("^[0-9]{2}$")]
        public int Age { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [StringLength(1000)]
        public string Qualification { get; set; }
        [StringLength(2000)]
        public string Experience { get; set; }
        [StringLength(4000)]
        public string Comment { get; set; }
        [Required]
        public bool AgreementRodo { get; set; }

        public List<JobOfferEmployee> JobOfferEmployees { get; set; }

        public List<JobOffer> JobOffers()
        {
            var jobOffers = new List<JobOffer>();

            foreach (var join in JobOfferEmployees)
            {
                jobOffers.Add(join.JobOffer);
            }

            return jobOffers;
        }

        public Employee()
        {
            JobOfferEmployees = new List<JobOfferEmployee>();
        }
    }
}
