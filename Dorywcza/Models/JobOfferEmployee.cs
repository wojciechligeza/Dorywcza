namespace Dorywcza.Models
{
    public class JobOfferEmployee
    {
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
