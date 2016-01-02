using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IListingRepository : IRepository<Listing>
    {
        IEnumerable<Listing> ListingForUser(int id, bool orderByDesc = true);
        int GetCount();
        IEnumerable<Listing> Page(int skip, int take, bool orderByDescDate = true);
    }
}
