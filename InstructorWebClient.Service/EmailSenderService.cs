using InstructorWebClient.Infrastructure.IService;
using System.Net;
using System.Net.Mail;

namespace InstructorWebClient.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        public void SendEmail(string htmlBody, string toEmail)
        {
            try
            {
                using (var smtpClient = new SmtpClient("mail.mitsolutions.in")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mail@mitsolutions.in", "mAiL#M@20!?"),
                    EnableSsl = true
                })
                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress("mail@mitsolutions.in", "Instructor Support"),
                    Body = htmlBody,
                    IsBodyHtml = true
                })
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception)
            {
                throw;                
            }
        }
    }
}
