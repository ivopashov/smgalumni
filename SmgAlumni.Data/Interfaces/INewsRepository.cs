using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface INewsRepository : IRepository<News>
    {
        IEnumerable<News> Page(int skip, int take);
        int GetCount();
    }
}
