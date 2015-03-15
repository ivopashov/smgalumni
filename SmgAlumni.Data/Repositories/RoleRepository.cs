using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(DbContext context):base(context)
        {

        }

        //just in case trim the parameter and compare string after tolower-ing them
        public Role GetByName(string name)
        {
            return this.Find(a => a.Name.Equals(name)).SingleOrDefault();
        }
    }
}
