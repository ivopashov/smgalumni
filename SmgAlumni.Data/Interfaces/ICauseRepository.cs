using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface ICauseRepository : IRepository<Cause>
    {
        int GetCount();
        IEnumerable<Cause> Page(int skip, int take, bool orderByDescDate = true);
    }
}
