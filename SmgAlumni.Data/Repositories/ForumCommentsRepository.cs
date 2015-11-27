using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public ForumComment GetById(int id)
        {
            return this._context.Comments.Find(id);
        }

        public void Delete(ForumComment entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(ForumComment entity)
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
