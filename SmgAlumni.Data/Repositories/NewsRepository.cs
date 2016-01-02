using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;
using System.Collections.Generic;

namespace SmgAlumni.Data.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly SmgAlumniContext _context;

        public NewsRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(News entity)
        {
            _context.NewsCollection.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(News entity)
        {
            _context.NewsCollection.Remove(entity);
            Save();
        }

        public News GetById(int id)
        {
            return _context.NewsCollection.Find(id);
        }

        public void Update(News entity, bool save = true)
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

        public int GetCount()
        {
            return _context.Listings.Count();
        }

        public IEnumerable<News> Page(int skip, int take, bool orderByDescDate = true)
        {
            if (orderByDescDate)
            {
                return _context.NewsCollection.OrderByDescending(a => a.DateCreated).Skip(skip).Take(take).ToList();
            }
            return _context.NewsCollection.OrderBy(a => a.DateCreated).Skip(skip).Take(take).ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
