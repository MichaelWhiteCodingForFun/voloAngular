using Microsoft.AspNet.Identity;
using POD.Data.Dapper;
using POD.Utilities;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace POD.Portal.Models.Auth.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridAsync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridAsync(IdentityMessage message)
        {
            /*
            int smtpPort;
            string smtpHost = PODEnvironment.GetSetting("stmpServer");
            string smtpUsername = PODEnvironment.GetSetting("emailService:Login");
            string smtpPassword = PODEnvironment.GetSetting("emailService:Password");
            string fromDisplayName = PODEnvironment.GetSetting("emailService:DisplayName");
            if (!Int32.TryParse(PODEnvironment.GetSetting("emailService:From"), out smtpPort))
            {
                smtpPort = 587;
            }
            string From = PODEnvironment.GetSetting("emailService:From");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(From, fromDisplayName);
            mailMessage.To.Add(message.Destination);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = true;

            SmtpClient emailClient = new SmtpClient(smtpHost, smtpPort);
            emailClient.EnableSsl = true;
            if (!String.IsNullOrEmpty(smtpUsername) && !String.IsNullOrEmpty(smtpPassword))
            {
                emailClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            }
            emailClient.Send(mailMessage);
            */
            await EmailManager.SendEmailAsync(message.Destination, message.Subject, message.Body);

        }


    }
}