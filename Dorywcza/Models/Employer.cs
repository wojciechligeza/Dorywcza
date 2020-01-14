using System.Collections.Generic;

namespace Dorywcza.Models
{
    public class Employer
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public List<JobOffer> JobOffers { get; set; }
        public int UserId { get; set; }
    }
}