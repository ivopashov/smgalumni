using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class NewsRepository : GenericRepository<News>
    {
        public NewsRepository(SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
