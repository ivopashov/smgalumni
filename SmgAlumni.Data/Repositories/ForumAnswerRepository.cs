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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public ForumAnswer GetById(int id)
        {
            return this._context.Answers.Find(id);
        }

        public void Delete(ForumAnswer entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(ForumAnswer entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Save()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
    }
}
