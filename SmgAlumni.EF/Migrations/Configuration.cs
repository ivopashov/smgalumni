namespace SmgAlumni.EF.Migrations
{
    using SmgAlumni.EF.Models;
    using SmgAlumni.EF.Models.enums;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SmgAlumni.EF.DAL.SmgAlumniContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SmgAlumni.EF.DAL.SmgAlumniContext";
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(SmgAlumni.EF.DAL.SmgAlumniContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            

            List<User> users = new List<User>()
            {
                new User()
                {
                    UserName = "ivopashov",
                    Email = "ivopashov@abv.bg",
                    YearOfGraduation = 2005,
                    Password = "/VHOxTfur8M+4m6M8xTIkW12uZgEv+jzN+eltLO+fhwyr2Q/PC/6BYjTbXBFqIWCEkm0jgCPLtxASciFWWK2vQ==",
                    PasswordSalt = "4e1bFqSrVt9tZJgF7e/aig/w23mRLQbxqRLGMjNrpLVSZLsgpQPcWr2E/mP+nweoPQvLXBvIvD6/1s+9AX0/ng==",
                    Division = ClassDivision.А,
                    Roles = new List<Role>()
                {
                    new Role()
                    {
                        Name="MasterAdmin"
                    },
                    new Role()
                    {
                        Name="Admin"
                    },
                    new Role(){
                    Name="User"
                    }
                },
                    DateJoined = DateTime.Now,
                    Verified = true,
                    FirstName = "Ivaylo",
                    MiddleName = "Dinkov",
                    LastName = "Pashov"

                }
            };

            users.ForEach(a => context.Users.AddOrUpdate(c=>c.Email, a));
            context.SaveChanges();

            List<Setting> settings = new List<Setting>() 
            {
                 new Setting()
                {
                    SettingKey = "auth_TokenExpirationMinutes",
                    SettingName = "120"
                },
                new Setting()
                {
                    SettingKey = "email_SmtpLogin",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "email_SmtpPassword",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "email_FromAddress",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "email_SmtpHost",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "email_SmtpPort",
                    SettingName = "150"
                },
                new Setting()
                {
                    SettingKey = "email_UseSecureConnection",
                    SettingName = "true"
                },
                new Setting()
                {
                    SettingKey = "mailgun_ApiUrl",
                    SettingName = "https://api.mailgun.net/v3/www.smg-alumni.com"
                },
                new Setting()
                {
                    SettingKey = "mailgun_ApiKey",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "mailgun_NewsLetterMailingList",
                    SettingName = "test"
                },
                new Setting()
                {
                    SettingKey = "mailgun_BaseUrl",
                    SettingName = "https://api.mailgun.net/v3"
                }
            };

            settings.ForEach(a => context.Settings.AddOrUpdate(c=>c.SettingKey, a));
            context.SaveChanges();
           
        }
    }
}
