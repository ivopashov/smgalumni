using SmgAlumni.Data.Interfaces;

namespace SmgAlumni.Utils.Settings
{
    public class EFSettingsRetriever : IAppSettingsRetriever
    {
        private readonly ISettingRepository _settingRepository;

        public EFSettingsRetriever(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public string GetSetting(string settingKey)
        {
            return _settingRepository.GetValueByKey(settingKey);
        }
    }
}
