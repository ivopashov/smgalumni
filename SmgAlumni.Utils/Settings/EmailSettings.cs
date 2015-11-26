using System;
using System.Net;
using System.Net.Mail;

namespace SmgAlumni.Utils.Settings
{
    public class EmailSettings
    {
        private readonly IAppSettingsRetriever _retriever;
        private readonly string _settingPrefix;

        public EmailSettings(IAppSettingsRetriever retriever)
        {
            if (retriever == null) throw new ArgumentNullException("retriever");

            _retriever = retriever;
            _settingPrefix = "email_";
        }

        public SmtpClient GetSmtpClient()
        {
            return new SmtpClient(SmtpHost, SmtpPort)
            {
                Credentials = new NetworkCredential(SmtpLogin, SmtpPassword),
                EnableSsl = UseSecureConnection
            };
        }

        public string SmtpHost
        {
            get { return _retriever.GetSetting(_settingPrefix + "SmtpHost"); }
        }
        public int SmtpPort
        {
            get { return Convert.ToInt16(_retriever.GetSetting(_settingPrefix + "SmtpPort")); }
        }
        public string FromAddress
        {
            get { return _retriever.GetSetting(_settingPrefix + "FromAddress"); }
        }
        public string SmtpLogin
        {
            get { return _retriever.GetSetting(_settingPrefix + "SmtpLogin"); }
        }
        public string SmtpPassword
        {
            get { return _retriever.GetSetting(_settingPrefix + "SmtpPassword"); }
        }
        public bool UseSecureConnection
        {
            get { return Convert.ToBoolean(_retriever.GetSetting(_settingPrefix + "UseSecureConnection")); }
        }
    }
}
