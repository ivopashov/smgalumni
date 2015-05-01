using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ForumThreadRepository : GenericRepository<ForumThread>
    {
        public ForumThreadRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
