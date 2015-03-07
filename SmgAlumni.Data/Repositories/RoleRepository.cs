using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    class RoleRepository : GenericRepository<Role>
    {
        //just in case trim the parameter and compare string after tolower-ing them
        public Role GetByName(string name)
        {
            return this.Find(a => a.Name.ToLower().Equals(name.Trim().ToLower())).SingleOrDefault();
        }
    }
}
