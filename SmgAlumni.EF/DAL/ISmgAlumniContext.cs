using System.Data.Entity;
using SmgAlumni.EF.Models;

namespace SmgAlumni.EF.DAL
{
    public interface ISmgAlumniContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<PasswordReset> PasswordResets { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<Cause> Causes { get; set; }
        DbSet<Listing> Listings { get; set; }
        DbSet<News> NewsCollection { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<ForumThread> Threads { get; set; }
        DbSet<ForumAnswer> Answers { get; set; }
        DbSet<ForumComment> Comments { get; set; }
        int SaveChanges();
    }
}
