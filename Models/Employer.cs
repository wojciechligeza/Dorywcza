using System.Collections.Generic;

namespace Dorywcza.Models
{
    public class Employer
    {
        public int EmployerId { get; set; }

        public string CompanyName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}