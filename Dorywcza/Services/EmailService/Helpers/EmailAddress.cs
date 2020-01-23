namespace Dorywcza.Services.EmailService.Helpers
{
    public class EmailAddress
    {
        public string Name { get; set; } = "Dorywcza.pl";
        public string Address { get; set; } = "wojciech@dorywcza.pl";

        public EmailAddress() { }
        public EmailAddress(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
