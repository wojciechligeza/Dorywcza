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
        public int Age { get; set; }
        [EmailAddress]
        public string Email { get; set; }
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
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
