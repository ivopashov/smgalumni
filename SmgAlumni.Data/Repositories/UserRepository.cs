using System;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmgAlumniContext _context;

        public UserRepository(SmgAlumniContext context)
        {
            _context = context;
        }        
        public int Add(User entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return this._context.Users.Find(id);
        }

        public void Update(User entity)
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
