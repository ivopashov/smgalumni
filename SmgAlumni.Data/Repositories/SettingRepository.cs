using System;
using System.Linq;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Collections.Generic;

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
            _context.Settings.Add(entity);
            Save();
            return entity.Id;
        }

        public Setting GetById(int id)
        {
            return _context.Settings.Find(id);
        }

        public void Delete(Setting entity)
        {
            _context.Settings.Remove(entity);
            Save();
        }

        public void Update(Setting entity)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Activity) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public string GetValueByKey(string key)
        {
            var setting = _context.Settings.Where(a => a.SettingKey == key).SingleOrDefault();

            if (setting == null)
            {
                return string.Empty;
            }

            return setting.SettingName;
        }

        public IEnumerable<Setting> GetAll()
        {
            return _context.Settings.ToList();
        }
    }
}
