using System;
using System.Collections.Generic;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class ForumThreadRepository : IForumThreadRepository
    {
        private readonly SmgAlumniContext _context;

        public ForumThreadRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(ForumThread entity)
        {
            _context.ForumThreads.Add(entity);
            Save();
            return entity.Id;
        }

        public void Delete(ForumThread entity)
        {
            _context.ForumThreads.Remove(entity);
            Save();
        }

        public ForumThread GetById(int id)
        {
            return _context.ForumThreads.Find(id);
        }

        public void Update(ForumThread entity)
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

        public IEnumerable<ForumThread> Page(int skip, int take)
        {
            return _context.ForumThreads.OrderBy(a => a.CreatedOn).Skip(skip).Take(take).ToList();
        }

        public int GetCount()
        {
            return _context.ForumThreads.Count();
        }
    }
}
