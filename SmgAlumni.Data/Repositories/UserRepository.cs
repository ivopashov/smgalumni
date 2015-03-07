using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public User GetById(int id)
        {
            return this.Find(a => a.Id == id).SingleOrDefault();
        }
    }
}
