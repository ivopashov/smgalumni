using System;
using System.Linq;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SmgAlumniContext _context;

        public RoleRepository(SmgAlumniContext context)
        {
            _context = context;
        }

        public void Save()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public void Update(Role entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public Role GetById(int id)
        {
            return this._context.Roles.Find(id);
        }

        public void Delete(Role entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public int Add(Role entity)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        //just in case trim the parameter and compare string after tolower-ing them
        public Role GetByName(string name)
        {
            return this._context.Roles.Where(a => a.Name.Equals(name)).SingleOrDefault();
        }
    }
}
