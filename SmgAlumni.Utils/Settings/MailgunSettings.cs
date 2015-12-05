using System;

namespace SmgAlumni.Utils.Settings
{
    public class MailgunSettings
    {
        private readonly IAppSettingsRetriever _retriever;
        private readonly string _settingPrefix;

        public MailgunSettings(IAppSettingsRetriever retriever)
        {
            if (retriever == null)
            {
                throw new ArgumentNullException("retriever");
            }

            _retriever = retriever;
            _settingPrefix = "mailgun_";
        }

        public string ApiUrl
        {
            get { return _retriever.GetSetting(_settingPrefix + "ApiUrl"); }
        }

        public string ApiKey
        {
            get { return _retriever.GetSetting(_settingPrefix + "ApiKey"); }
        }

        public string NewsLetterMailingList
        {
            get { return _retriever.GetSetting(_settingPrefix + "NewsLetterMailingList"); }
        }

        public string BaseUrl
        {
            get { return _retriever.GetSetting(_settingPrefix + "BaseUrl"); }
        }

        public string From
        {
            get { return _retriever.GetSetting(_settingPrefix + "From"); }
        }

        public string Subject
        {
            get { return _retriever.GetSetting(_settingPrefix + "Subject"); }
        }
    }
}
