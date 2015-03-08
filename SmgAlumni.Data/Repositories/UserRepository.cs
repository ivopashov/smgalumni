using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext context):base(context)
        {

        }        
    }
}
