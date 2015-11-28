using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        IEnumerable<Setting> GetAll();
        string GetValueByKey(string key);
    }
}
