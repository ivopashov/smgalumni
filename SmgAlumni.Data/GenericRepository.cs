using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data
{
    public abstract class GenericRepository<T> : IRepository<T> where T:class, IEntity
    {
        private DbSet<T> EntitySet;
        private DbContext _context;

        public GenericRepository(DbContext context)
        {
            EntitySet = context.Set<T>();
            _context = context;
        }

        public T GetById(int id)
        {
            return this.Find(a => a.Id == id).SingleOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            return EntitySet;
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return EntitySet.Where(predicate);
        }

        public int Add(T entity)
        {
            EntitySet.Add(entity);
            Save();
            return entity.Id;
        }

        public void Update(T entity)
        {
            var oldEntity = this.Find(a => a.Id == entity.Id).SingleOrDefault();
            if (oldEntity == null) throw new Exception("Could not find searched for object of type" + typeof(T)+ " with id "+entity.Id);

            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            
            Save();
        }

        public void Delete(T entity)
        {
            EntitySet.Remove(entity);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
