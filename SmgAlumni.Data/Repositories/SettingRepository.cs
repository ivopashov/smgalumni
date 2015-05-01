using System.Linq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class SettingRepository : GenericRepository<Setting>
    {
        public SettingRepository(SmgAlumniContext context)
            : base(context)
        {

        }

        public string GetValueByKey(string key)
        {
            var setting= this.GetAll().Where(a => a.SettingKey == key).SingleOrDefault();
            if (setting == null) return string.Empty;

            return setting.SettingName;
        }
    }
}
