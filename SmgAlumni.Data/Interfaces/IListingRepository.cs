using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IListingRepository : IRepository<Listing>
    {
        IEnumerable<Listing> PageListingForUser(int id, int skip, int take, bool orderByDesc = true);
        int GetCount(int? userId = null);
        IEnumerable<Listing> Page(int skip, int take, bool orderByDescDate = true);
    }
}
