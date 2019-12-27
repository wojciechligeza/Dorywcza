﻿namespace Dorywcza.Services.EmailService
{
    public class EmailAddress
    {
        public string Name { get; set; } = "Wojciech";
        public string Address { get; set; } = "covalig.ligese@gmail.com";

        public EmailAddress() { }
        public EmailAddress(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
