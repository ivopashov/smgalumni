using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ForumCommentsRepository : GenericRepository<ForumComment>
    {
        public ForumCommentsRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
