using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(SmgAlumniContext context)
            : base(context)
        {

        }        
    }
}
