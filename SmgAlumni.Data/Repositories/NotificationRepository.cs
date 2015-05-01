using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>
    {
        public NotificationRepository(SmgAlumniContext context)
            : base(context)
        {

        }        
    }
}
