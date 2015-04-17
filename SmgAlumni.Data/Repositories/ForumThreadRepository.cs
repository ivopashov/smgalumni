using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.Data.Repositories
{
    public class ForumThreadRepository : GenericRepository<ForumThread>
    {
        public ForumThreadRepository(SmgAlumni.EF.DAL.SmgAlumniContext context)
            : base(context)
        {

        }
    }
}
