using System;
using SmgAlumni.EF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SmgAlumni.Data.Repositories
{
    public class SettingRepository : GenericRepository<Setting>
    {
        public SettingRepository(SmgAlumni.EF.DAL.SmgAlumniContext context)
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
