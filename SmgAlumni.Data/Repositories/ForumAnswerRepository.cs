using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ForumAnswerRepository : IForumAnswerRepository
    {
        private readonly SmgAlumniContext _context;

        public ForumAnswerRepository(SmgAlumniContext context)
        {
            _context = context;
        }
        public int Add(ForumAnswer entity)
        {
            _context.ForumAnswers.Add(entity);
            Save();
            return entity.Id;
        }

        public ForumAnswer GetById(int id)
        {
            return _context.ForumAnswers.Find(id);
        }

        public void Delete(ForumAnswer entity)
        {
            _context.ForumAnswers.Remove(entity);
            Save();
        }

        public void Update(ForumAnswer entity, bool save = true)
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
