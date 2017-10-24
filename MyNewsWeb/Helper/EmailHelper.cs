using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MyNewsWeb.Helper
{
    public class EmailHelper
    {
        public static readonly string EMAIL_CREDENTIALS = "";
        public static readonly string SMTP_CLIENT = "smtp.gmail.com";

        public bool SendEmail(string sender, string[] recipients, string subject, string message)
        {
            bool isMessageSent = false;
            //initialize param
            SmtpClient client = new SmtpClient(SMTP_CLIENT);
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            NetworkCredential credentials = new NetworkCredential(sender, EMAIL_CREDENTIALS);
            client.EnableSsl = true;
            client.Credentials = credentials;
            try
            {
                var receivers = string.Join(";", recipients);
                var mail = new MailMessage(sender.Trim(), string.Join(",", recipients));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                client.Send(mail);
                isMessageSent = true;
            }
            catch(Exception ex)
            {
                isMessageSent = false;
            }
            return isMessageSent;
        }
    }
}
