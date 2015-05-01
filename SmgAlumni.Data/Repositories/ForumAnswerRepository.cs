using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class ForumAnswerRepository : GenericRepository<ForumAnswer>
    {
        public ForumAnswerRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
