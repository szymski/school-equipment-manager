using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SchoolEquipmentManager.Models;

namespace SchoolEquipmentManager.Logic
{
    public class EmailServiceOptions
    {
        public bool Enabled { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class EmailService : IEmailService
    {
        private EmailServiceOptions _options;

        public EmailService(IOptions<EmailServiceOptions> options)
        {
            _options = options.Value;
        }

        public void SendEmail(ApplicationUser recipent, string title, string body)
        {
            if(!_options.Enabled)
                return;

            if(string.IsNullOrEmpty(recipent.Email))
                return;

            SendEmail(recipent.Email, title, body);
        }

        public void SendEmail(string address, string title, string body)
        {
            if (!_options.Enabled)
                return;

            SmtpClient client = new SmtpClient(_options.Host, _options.Port);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(_options.Email, _options.Password);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_options.Email);
            message.To.Add(address);
            message.Subject = title;
            message.Body = body;
            message.IsBodyHtml = true;

            client.Send(message);
        }

    }
}
