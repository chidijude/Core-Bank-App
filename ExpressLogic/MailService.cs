using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ExpressLogic
{
    public static class MailService
    {
        public static void SendEmail(string emailBody, string userEmail)
        {
            MailMessage mailMessage = new MailMessage("judechidiemmanuel4kz@gmail.com", userEmail);
            mailMessage.Subject = "Express Core User Account Creation";
            mailMessage.Body = emailBody;

            var smtp = new SmtpClient
            {
                
                DeliveryMethod = SmtpDeliveryMethod.Network,
                
            };

            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.EnableSsl = true;
            smtp.Send(mailMessage);
        }

        /*public async Task  SendEmail(string userEmail, string UserName, string message)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("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");

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("judechidiemmanuel4kz@gmail.com", "Express Admin")); //sender mail and User name

            var recipients = new List<EmailAddress>
            {
                new EmailAddress(userEmail, UserName), // new user email and name 
                
            };
            msg.AddTos(recipients);

            msg.SetSubject("Express Core User Account Creation");

            msg.AddContent(MimeType.Html, "<p>Hello!</p>");
            msg.AddContent(MimeType.Text, message );
            var response await client.SendEmailAsync(msg);

           
           

        }*/
        public static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("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");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("judechidiemmanuel4kz@gmail.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("judechidiemmanuel4kz@gmail.com", "Example1 User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }/*<system.net>
    <mailSettings>
      <smtp deliveryMethod = "Network" >
        < network host="smtp.sendgrid.net" port="465" userName="emailsender" password="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
" enableSsl="true" />
      </smtp>
    </mailSettings>
  </system.net>*/
}
