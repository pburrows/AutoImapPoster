
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ImapPoster.Mail
{
    public class MessageHandler
    {
        public string BuildMessageBody(ReadabilityParserResponse parsedArticle, string comment)
        {
            string bodyFormat = "<p>{0}</p><p>{1}</p><p>{2}</p>";
            string body = string.Format(bodyFormat, parsedArticle.title, parsedArticle.url, parsedArticle.content);
            if (string.IsNullOrEmpty(comment)) return body;
            body = string.Format("<p>{0}</p><p>{1}</p>", comment, body);
            return body;
        }

        public ReadabilityParserResponse ParseArticle(string articleUrl)
        {
            string baseUrl = ConfigurationManager.AppSettings["ReadbilityBaseUrl"];
            string token = ConfigurationManager.AppSettings["ReadabilityParserApi_Token"];

            string urlFormat = "{0}?token={1}&url={2}";
            string url = string.Format(urlFormat, baseUrl, token, articleUrl);

            try
            {
                using (var wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    string result = wc.DownloadString(url);
                    return JsonConvert.DeserializeObject<ReadabilityParserResponse>(result);
                }

            }
            catch
            {
            }
            
            return null;

        }
    }
}
