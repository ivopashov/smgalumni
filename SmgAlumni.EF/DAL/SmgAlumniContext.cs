using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.EF.DAL
{
    public class SmgAlumniContext : DbContext
    {
        public SmgAlumniContext()
            : base("SmgAlumniContext")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PasswordReset> PasswordResets{ get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Cause> Causes{ get; set; }
        public DbSet<News> NewsCollection { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
