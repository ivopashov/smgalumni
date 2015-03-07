using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data
{
    public abstract class GenericRepository<T> : IRepository<T> where T:class
    {
        private DbSet<T> EntitySet;
        private DbContext _context;

        public GenericRepository(DbContext context)
        {
            EntitySet = context.Set<T>();
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return EntitySet;
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return EntitySet.Where(predicate);
        }

        public void Add(T entity)
        {
            EntitySet.Add(entity);
        }

        public void Delete(T entity)
        {
            EntitySet.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
