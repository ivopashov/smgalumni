using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;

namespace SmgAlumni.EF.DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<SmgAlumniContext>
    {
        protected override void Seed(SmgAlumniContext context)
        {
            var user = new User()
            {
                UserName = "ivopashov",
                Email = "ivopashov@abv.bg",
                YearOfGraduation = 2005,
                Password = "/VHOxTfur8M+4m6M8xTIkW12uZgEv+jzN+eltLO+fhwyr2Q/PC/6BYjTbXBFqIWCEkm0jgCPLtxASciFWWK2vQ==",
                PasswordSalt = "4e1bFqSrVt9tZJgF7e/aig/w23mRLQbxqRLGMjNrpLVSZLsgpQPcWr2E/mP+nweoPQvLXBvIvD6/1s+9AX0/ng==",
                Division = ClassDivision.Г,
                Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name=RoleType.MasterAdmin
                    }   
                }
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
