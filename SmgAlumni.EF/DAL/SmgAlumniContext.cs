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
        public virtual DbSet<Notification> PendingNotifications { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ForumThread> ForumThreads { get; set; }
        public virtual DbSet<ForumAnswer> ForumAnswers { get; set; }
        public virtual DbSet<ForumComment> ForumComments { get; set; }
        public virtual DbSet<NewsLetterCandidate> NewsLetterCandidates { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
