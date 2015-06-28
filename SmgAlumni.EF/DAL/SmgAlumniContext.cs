using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SmgAlumni.EF.Models;

namespace SmgAlumni.EF.DAL
{
    public class SmgAlumniContext : DbContext, ISmgAlumniContext
    {
        public SmgAlumniContext()
            : base("name=SmgAlumniContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Cause> Causes { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<News> NewsCollection { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ForumThread> Threads { get; set; }
        public DbSet<ForumAnswer> Answers { get; set; }
        public DbSet<ForumComment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
