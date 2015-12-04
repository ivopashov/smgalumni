using System;

namespace SmgAlumni.Utils.Settings
{
    public class NewsLetterSettings
    {
        private readonly IAppSettingsRetriever _retriever;
        private readonly string _settingPrefix;

        public NewsLetterSettings(IAppSettingsRetriever retriever)
        {
            if (retriever == null)
            {
                throw new ArgumentNullException("retriever");
            }

            _retriever = retriever;
            _settingPrefix = "newsletter_";
        }


        public string SiteUrl
        {
            get { return _retriever.GetSetting(_settingPrefix + "SiteUrl"); }
        }

        public string BiWeeklyNewsLetterTemplatePath
        {
            get { return _retriever.GetSetting(_settingPrefix + "BiWeeklyNewsLetterTemplatePath"); }
        }
    }
}
