using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly SmgAlumniContext _context;

        public ActivityRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public int Add(Activity entity)
        {
            _context.Activities.Add(entity);
            Save();
            return entity.Id;
        }

        public Activity GetById(int id)
        {
            return _context.Activities.Find(id);
        }

        public void Delete(Activity entity)
        {
            _context.Activities.Remove(entity);
            Save();
        }

        public void Update(Activity entity, bool save = true)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Activity) + " with id " + entity.Id);
            }

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            if (save)
            {
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
