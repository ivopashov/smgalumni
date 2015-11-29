using SmgAlumni.EF.Models.enums;
using System;
using System.Collections.Generic;

namespace SmgAlumni.EF.Models
{
    public class User : IEntity
    {
        public User()
        {
            PasswordResets = new List<PasswordReset>();
            Roles = new List<Role>();
            News = new List<News>();
            Listings = new List<Listing>();
            Causes = new List<Cause>();
            ForumAnswers = new List<ForumAnswer>();
            ForumComments = new List<ForumComment>();
            ForumThreads = new List<ForumThread>();
            Activities = new List<Activity>();
        }

        //basic identification
        public int Id { get; set; }
        public string UserName { get; set; }
        public int YearOfGraduation { get; set; }
        public ClassDivision Division { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        //optional
        public string DwellingCountry { get; set; }
        public string UniversityGraduated { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public virtual List<NotificationSubscription> NotificationSubscriptions { get; set; }

        //for admin usage
        public bool Verified { get; set; }
        public DateTime DateJoined { get; set; }
        public virtual List<PasswordReset> PasswordResets { get; set; }

        //image
        public byte[] AvatarImage { get; set; }

        //role
        public virtual List<Role> Roles { get; set; }
        public virtual List<News> News { get; set; }
        public virtual List<Cause> Causes { get; set; }
        public virtual List<Listing> Listings { get; set; }
        public virtual List<ForumThread> ForumThreads { get; set; }
        public virtual List<ForumAnswer> ForumAnswers { get; set; }
        public virtual List<ForumComment> ForumComments { get; set; }
        public virtual List<Activity> Activities { get; set; }
    }
}
