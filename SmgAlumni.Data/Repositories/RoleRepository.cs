using System.Linq;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;

namespace SmgAlumni.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(SmgAlumniContext context)
            : base(context)
        {

        }

        //just in case trim the parameter and compare string after tolower-ing them
        public Role GetByName(string name)
        {
            return this.Find(a => a.Name.Equals(name)).SingleOrDefault();
        }
    }
}
