using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebLightNovel.Models.SystemModel
{
    public class SendEmail
    {
        public static bool Sendmail(string to, string subject, string body, string attackfile)
        {
            try
            {
                MailMessage msg = new MailMessage(InforEmail.emailSender, to, subject, body);
                using (var client = new SmtpClient(InforEmail.hostEmail, InforEmail.portEmail))
                {
                    client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attackfile))
                    {
                        Attachment attachment = new Attachment(attackfile);
                        msg.Attachments.Add(attachment);
                    }
                    NetworkCredential credential = new NetworkCredential(InforEmail.emailSender, InforEmail.password);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.Send(msg);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
                return false;
            }
            return true;
        }

    }
}