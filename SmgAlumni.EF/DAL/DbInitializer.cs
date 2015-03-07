using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SmgAlumni.EF.Models;

namespace SmgAlumni.EF.DAL
{
    public class DbInitializer : CreateDatabaseIfNotExists<SmgAlumniContext>
    {
        protected override void Seed(SmgAlumniContext context)
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name="Admin"
                },
                new Role()
                {
                    Name="MasterAdmin"
                },
                new Role()
                {
                    Name="User"
                }
            };

            roles.ForEach(a => context.Roles.Add(a));
            context.SaveChanges();
        }
    }
}
