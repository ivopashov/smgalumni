namespace SmgAlumni.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityType = c.Int(nullable: false),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        YearOfGraduation = c.Int(nullable: false),
                        Division = c.Int(nullable: false),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        DwellingCountry = c.String(),
                        UniversityGraduated = c.String(),
                        Profession = c.String(),
                        Company = c.String(),
                        Description = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        City = c.String(),
                        AddedToNewsLetterList = c.Boolean(nullable: false),
                        Verified = c.Boolean(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        AvatarImage = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cause",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ForumAnswer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        Body = c.String(),
                        Likes = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ForumThreadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumThread", t => t.ForumThreadId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ForumThreadId);
            
            CreateTable(
                "dbo.ForumComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        ForumAnswerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumAnswer", t => t.ForumAnswerId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ForumAnswerId);
            
            CreateTable(
                "dbo.ForumThread",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        ForumThread_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumThread", t => t.ForumThread_Id)
                .Index(t => t.ForumThread_Id);
            
            CreateTable(
                "dbo.Listing",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TempKey = c.Guid(),
                        Size = c.Int(nullable: false),
                        Description = c.String(),
                        CreatedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Data = c.Binary(),
                        Listing_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Listing", t => t.Listing_Id)
                .Index(t => t.Listing_Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.NewsLetterCandidate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HtmlBody = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        SentOn = c.DateTimeOffset(nullable: false, precision: 7),
                        Type = c.Int(nullable: false),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.PasswordReset",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Used = c.Boolean(nullable: false),
                        Guid = c.Guid(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        SentOn = c.DateTime(),
                        Sent = c.Boolean(nullable: false),
                        Message = c.Binary(),
                        HtmlMessage = c.String(),
                        To = c.String(),
                        Kind = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SettingKey = c.String(),
                        SettingName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Role", "User_Id", "dbo.User");
            DropForeignKey("dbo.PasswordReset", "User_Id", "dbo.User");
            DropForeignKey("dbo.NewsLetterCandidate", "CreatedBy_Id", "dbo.User");
            DropForeignKey("dbo.News", "User_Id", "dbo.User");
            DropForeignKey("dbo.Listing", "User_Id", "dbo.User");
            DropForeignKey("dbo.Attachment", "Listing_Id", "dbo.Listing");
            DropForeignKey("dbo.ForumAnswer", "UserId", "dbo.User");
            DropForeignKey("dbo.ForumThread", "UserId", "dbo.User");
            DropForeignKey("dbo.Tag", "ForumThread_Id", "dbo.ForumThread");
            DropForeignKey("dbo.ForumAnswer", "ForumThreadId", "dbo.ForumThread");
            DropForeignKey("dbo.ForumComment", "UserId", "dbo.User");
            DropForeignKey("dbo.ForumComment", "ForumAnswerId", "dbo.ForumAnswer");
            DropForeignKey("dbo.Cause", "User_Id", "dbo.User");
            DropForeignKey("dbo.Activity", "User_Id", "dbo.User");
            DropIndex("dbo.Role", new[] { "User_Id" });
            DropIndex("dbo.PasswordReset", new[] { "User_Id" });
            DropIndex("dbo.NewsLetterCandidate", new[] { "CreatedBy_Id" });
            DropIndex("dbo.News", new[] { "User_Id" });
            DropIndex("dbo.Attachment", new[] { "Listing_Id" });
            DropIndex("dbo.Listing", new[] { "User_Id" });
            DropIndex("dbo.Tag", new[] { "ForumThread_Id" });
            DropIndex("dbo.ForumThread", new[] { "UserId" });
            DropIndex("dbo.ForumComment", new[] { "ForumAnswerId" });
            DropIndex("dbo.ForumComment", new[] { "UserId" });
            DropIndex("dbo.ForumAnswer", new[] { "ForumThreadId" });
            DropIndex("dbo.ForumAnswer", new[] { "UserId" });
            DropIndex("dbo.Cause", new[] { "User_Id" });
            DropIndex("dbo.Activity", new[] { "User_Id" });
            DropTable("dbo.Setting");
            DropTable("dbo.Notification");
            DropTable("dbo.Role");
            DropTable("dbo.PasswordReset");
            DropTable("dbo.NewsLetterCandidate");
            DropTable("dbo.News");
            DropTable("dbo.Attachment");
            DropTable("dbo.Listing");
            DropTable("dbo.Tag");
            DropTable("dbo.ForumThread");
            DropTable("dbo.ForumComment");
            DropTable("dbo.ForumAnswer");
            DropTable("dbo.Cause");
            DropTable("dbo.User");
            DropTable("dbo.Activity");
        }
    }
}
