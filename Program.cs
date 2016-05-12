using ImapPoster.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ImapPoster
{
    class Program
    {
        private static Logger _logger = LogManager.GetLogger("ImapPoster.Program");

        static void Main(string[] args)
        {
            try
            {
                _logger.Info("Starting ImapPoster");
                var ms = new MailService();
                ms.ReadMailbox();

                _logger.Info("Finishing ImapPoster");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}
