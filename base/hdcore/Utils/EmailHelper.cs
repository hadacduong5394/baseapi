using System.Net;
using System.Net.Mail;

namespace hdcore.Utils
{
    public class EmailHelper
    {
        public static void SendEmail(string emailFrom, string emailTo, string subject, string content, string displayName, string emailPassword, string host, string port, bool ssl)
        {
            string body = content;
            MailMessage message = new MailMessage(new MailAddress(emailFrom), new MailAddress(emailTo));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(emailFrom, emailPassword);
            client.Host = host;
            client.EnableSsl = ssl;
            client.Port = int.Parse(port);
            client.Send(message);
        }
    }
}