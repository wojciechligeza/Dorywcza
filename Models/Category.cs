using System.Collections.Generic;

namespace Dorywcza.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string TypeOfJob { get; set; }
        public string Workplace { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}