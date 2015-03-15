using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.Settings
{
    public interface IAppSettingsRetriever
    {
        string GetSetting(string settingKey);
    }
}
