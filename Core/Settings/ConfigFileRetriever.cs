using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.Settings
{
    public class ConfigFileRetriever : IAppSettingsRetriever
    {
        public string GetSetting(string settingKey)
        {
            return ConfigurationManager.AppSettings[settingKey];
        }
    }
}
