using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class CauseRepository : GenericRepository<Cause>
    {
        public CauseRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
