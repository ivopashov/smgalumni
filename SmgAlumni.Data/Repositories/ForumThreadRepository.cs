using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Delete(ForumThread entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public ForumThread GetById(int id)
        {
            return this._context.Threads.Find(id);
        }

        public void Update(ForumThread entity)
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
