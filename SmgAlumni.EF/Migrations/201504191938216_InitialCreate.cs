namespace SmgAlumni.EF.Migrations
{
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
                "dbo.Listing",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Heading = c.String(),
                        Body = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                        Sent = c.Boolean(nullable: false),
                        Message = c.Binary(),
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
            DropForeignKey("dbo.News", "User_Id", "dbo.User");
            DropForeignKey("dbo.Listing", "User_Id", "dbo.User");
            DropForeignKey("dbo.ForumAnswer", "UserId", "dbo.User");
            DropForeignKey("dbo.ForumThread", "UserId", "dbo.User");
            DropForeignKey("dbo.ForumAnswer", "ForumThreadId", "dbo.ForumThread");
            DropForeignKey("dbo.ForumComment", "UserId", "dbo.User");
            DropForeignKey("dbo.ForumComment", "ForumAnswerId", "dbo.ForumAnswer");
            DropForeignKey("dbo.Cause", "User_Id", "dbo.User");
            DropForeignKey("dbo.Activity", "User_Id", "dbo.User");
            DropIndex("dbo.Role", new[] { "User_Id" });
            DropIndex("dbo.PasswordReset", new[] { "User_Id" });
            DropIndex("dbo.News", new[] { "User_Id" });
            DropIndex("dbo.Listing", new[] { "User_Id" });
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
            DropTable("dbo.News");
            DropTable("dbo.Listing");
            DropTable("dbo.ForumThread");
            DropTable("dbo.ForumComment");
            DropTable("dbo.ForumAnswer");
            DropTable("dbo.Cause");
            DropTable("dbo.User");
            DropTable("dbo.Activity");
        }
    }
}
