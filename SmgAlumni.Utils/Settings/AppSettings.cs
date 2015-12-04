using System;

namespace SmgAlumni.Utils.Settings
{
    public class AppSettings : IAppSettings
    {
        private AuthenticationSettings _authentication;
        private IAppSettingsRetriever _retriever;
        private MessagingSettings _messaging;
        private EmailSettings _email;
        private MailgunSettings _mailGunSettings;
        private NewsLetterSettings _newsLetterSettings;

        public AppSettings(IAppSettingsRetriever retriever)
        {
            if (retriever == null)
            {
                throw new ArgumentNullException("retriever");
            }

            _retriever = retriever;
        }

        public virtual AuthenticationSettings Authentication
        {
            get
            {
                return _authentication ?? (_authentication = new AuthenticationSettings(_retriever));
            }
        }

        public virtual MessagingSettings Messaging
        {
            get { return _messaging ?? (_messaging = new MessagingSettings()); }
        }

        public virtual EmailSettings Email
        {
            get { return _email ?? (_email = new EmailSettings(_retriever)); }
        }

        public virtual MailgunSettings MailgunSettings
        {
            get
            {
                return _mailGunSettings ?? (_mailGunSettings = new MailgunSettings(_retriever));
            }
        }

        public NewsLetterSettings NewsLetterSettings
        {
            get
            {
                return _newsLetterSettings ?? (_newsLetterSettings = new NewsLetterSettings(_retriever));
            }
        }
    }
}
