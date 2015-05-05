using SmgAlumni.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Utils.Settings
{
    public class EFSettingsRetriever : IAppSettingsRetriever
    {
        private readonly SettingRepository _settingRepository;

        public EFSettingsRetriever(SettingRepository settingRepository)
        {
            _settingRepository=settingRepository;
        }

        public string GetSetting(string settingKey)
        {
            return _settingRepository.GetValueByKey(settingKey);
        }
    }
}
