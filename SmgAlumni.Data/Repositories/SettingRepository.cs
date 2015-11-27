using System;
using System.Linq;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly SmgAlumniContext _context;

        public SettingRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public int Add(Setting entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Setting GetById(int id)
        {
            return this._context.Settings.Find(id);
        }

        public void Delete(Setting entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(Setting entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Save()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public string GetValueByKey(string key)
        {
            var setting= this._context.Settings.Where(a => a.SettingKey == key).SingleOrDefault();
            
            if (setting == null)
            {
                return string.Empty;
            }

            return setting.SettingName;
        }
    }
}
