namespace Dorywcza.Services.EmailService.Helpers
{
    public class EmailAddress
    {
        public string Name { get; set; } = "Dorywcza.pl";
        public string Address { get; set; } = "covalig.ligese@gmail.com";

        public EmailAddress() { }
        public EmailAddress(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
