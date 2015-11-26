using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SmgAlumni.EF.Models;

namespace SmgAlumni.EF.DAL
{
    public class SmgAlumniContext : DbContext
    {
        public SmgAlumniContext()
            : base("name=SmgAlumniContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<PasswordReset> PasswordResets { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Cause> Causes { get; set; }
        public virtual DbSet<Listing> Listings { get; set; }
        public virtual DbSet<News> NewsCollection { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ForumThread> Threads { get; set; }
        public virtual DbSet<ForumAnswer> Answers { get; set; }
        public virtual DbSet<ForumComment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
