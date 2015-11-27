namespace SmgAlumni.Data.Interfaces
{
    public interface IRepository<T> where T:class
    {
        int Add(T entity);
        T GetById(int id);
        void Delete(T entity);
        void Update(T entity);
        void Save();
    }
}
