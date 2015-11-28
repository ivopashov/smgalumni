using System;
using System.Collections.Generic;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using System.Linq;

namespace SmgAlumni.Data.Repositories
{
    public class ForumCommentsRepository : IForumCommentsRepository
    {
        private readonly SmgAlumniContext _context;

        public ForumCommentsRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public int Add(ForumComment entity)
        {
            _context.Comments.Add(entity);
            Save();
            return entity.Id;
        }

        public ForumComment GetById(int id)
        {
            return _context.Comments.Find(id);
        }

        public void Delete(ForumComment entity)
        {
            _context.Comments.Remove(entity);
            Save();
        }

        public void Update(ForumComment entity)
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

        public IEnumerable<ForumComment> GetCommentsForAnswer(int answerID)
        {
            return _context.Comments.Where(a => a.ForumAnswerId == answerID).ToList();
        }
    }
}
