using System;
using System.Collections.Generic;
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
            _context.SaveChanges();
        }

        public void Update(Role entity, bool save = true)
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

        public Role GetById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void Delete(Role entity)
        {
            _context.Roles.Remove(entity);
            Save();
        }

        public int Add(Role entity)
        {
            _context.Roles.Add(entity);
            Save();
            return entity.Id;
        }

        //just in case trim the parameter and compare string after tolower-ing them
        public Role GetByName(string name)
        {
            return _context.Roles.Where(a => a.Name.Equals(name)).SingleOrDefault();
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }
    }
}
