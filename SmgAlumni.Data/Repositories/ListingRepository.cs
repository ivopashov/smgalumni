using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ListingRepository : GenericRepository<Listing>
    {
        public ListingRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
