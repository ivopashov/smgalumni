using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Settings
{
    public class ConfigFileRetriever : IAppSettingsRetriever
    {
        public string GetSetting(string settingKey)
        {
            return ConfigurationManager.AppSettings[settingKey];
        }
    }
}
