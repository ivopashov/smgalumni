using System;

namespace SmgAlumni.Utils.Settings
{
    public class AuthenticationSettings
    {
        private readonly IAppSettingsRetriever _retriever;
        private readonly string _settingPrefix;

        public AuthenticationSettings(IAppSettingsRetriever retriever)
        {
            if (retriever == null) throw new ArgumentNullException("retriever");

            _retriever = retriever;
            _settingPrefix = "auth_";
        }

        public TimeSpan TokenExpirationLength
        {
            get { return TimeSpan.FromMinutes(Convert.ToInt32(_retriever.GetSetting(_settingPrefix + "TokenExpirationMinutes"))); }
        }
    }
}
