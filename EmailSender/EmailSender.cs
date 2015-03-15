using SmgAlumni.Utils.EmailQuerer.Serialization;
using SmgAlumni.Utils.Settings;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class EmailSender
    {
        private readonly AppSettings _appSettings;
        private readonly Logger _logger;

        public EmailSender(AppSettings appSettings)
        {
            if (appSettings == null) throw new ArgumentNullException("appSettings");

            _appSettings = appSettings;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public bool Send(MailMessage emailMessage)
        {
            var smtp = _appSettings.Email.GetSmtpClient();

            try
            {
                smtp.Send(emailMessage);
                _logger.Info("Email to: " + emailMessage.To + " was sent successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public bool Send(SerializeableMailMessage emailMessage)
        {
            return Send(emailMessage.GetMailMessage());
        }
    }
}
