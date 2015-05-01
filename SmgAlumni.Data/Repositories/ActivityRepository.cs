using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ActivityRepository : GenericRepository<Activity>
    {
        public ActivityRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
