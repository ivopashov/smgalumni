using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;
using System.Collections.Generic;

namespace SmgAlumni.Data.Repositories
{
    public class CauseRepository : ICauseRepository
    {
        private readonly SmgAlumniContext _context;

        public CauseRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public Cause GetById(int id)
        {
            return _context.Causes.Find(id);
        }

        public int Add(Cause entity)
        {
            _context.Causes.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(Cause entity)
        {
            _context.Causes.Remove(entity);
            Save();
        }

        public int GetCount()
        {
            return _context.Causes.Count();
        }

        public void Update(Cause entity, bool save = true)
        {
            var oldEntity = GetById(entity.Id);
            if (oldEntity == null)
            {
                throw new Exception("Could not find searched for object of type" + typeof(Cause) + " with id " + entity.Id);
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

        public IEnumerable<Cause> Page(int skip, int take)
        {
            return _context.Causes.OrderBy(a => a.DateCreated).Skip(skip).Take(take).ToList();
        }
    }
}
