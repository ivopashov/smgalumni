using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

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
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Delete(News entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public News GetById(int id)
        {
            return this._context.NewsCollection.Find(id);
        }

        public void Update(News entity)
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
