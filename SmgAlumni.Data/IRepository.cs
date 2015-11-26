using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmgAlumni.Data
{
    public interface IRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        int Add(T entity);
        void Delete(T entity);
        void Save();
    }
}
