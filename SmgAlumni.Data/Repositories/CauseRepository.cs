using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

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
            return this._context.Causes.Find(id);
        }

        public int Add(Cause entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Delete(Cause entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(Cause entity)
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
