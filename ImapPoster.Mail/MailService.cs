using NLog;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ImapPoster.Mail
{
    public class MailService
    {
        private static Logger _logger = LogManager.GetLogger("ImapPoster.Program");

        public void ReadMailbox()
        {
            string emailAddress = ConfigurationManager.AppSettings["GatewayEmailAddress"];
            string password = ConfigurationManager.AppSettings["GatewayEmailPassword"];
            using (var client = new ImapClient("imap.gmail.com", 993, emailAddress, password, ssl: true))
            {
                IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());
                IEnumerable<MailMessage> messages = client.GetMessages(uids);
                var mh = new MessageHandler();
                _logger.Info("Found {0} messages", uids.Count());
                foreach (var msg in messages)
                {
                    var url = ParseUrlFromEmailMessage(msg);

                    ReadabilityParserResponse resp = mh.ParseArticle(url.ToString());
                    string body = mh.BuildMessageBody(resp, msg.Subject);
                    string subject = resp.title;
                    SendMail(subject, body);
                    _logger.Info("Sent: {0}", subject);
                }
            }

        }

        private static StringBuilder ParseUrlFromEmailMessage(MailMessage msg)
        {
            //note: URL must be first line of email address. 
            //      A line that starts with two - characters stops parsing of the url

            var url = new StringBuilder();
            for (int i = 0; i < msg.Body.Length; i++)
            {
                char c = msg.Body[i];

                if (CheckForEndOfMessageSequence(msg, c, i))
                {
                    break;
                }

                url.Append(c);
            }
            return url;
        }

        private static bool CheckForEndOfMessageSequence(MailMessage msg, char c, int i)
        {
            if (c == '\n' || c == '\r')
            {
                if (i + 2 < msg.Body.Length)
                {
                    if (msg.Body[i + 1] == '-' && msg.Body[i + 2] == '-')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void SendMail(string subject, string body)
        {
            string emailAddress = ConfigurationManager.AppSettings["EmailAddress"];
            string toAddress = ConfigurationManager.AppSettings["ToEmailAddress"];
            string password = ConfigurationManager.AppSettings["EmailAddressPassword"];



            MailMessage mm = new MailMessage(emailAddress, toAddress);
            mm.Subject = subject;
            mm.Body = body;
            mm.IsBodyHtml = true;

            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailAddress, password)
            })
            {
                smtp.Send(mm);
            }
        }
    }
}
