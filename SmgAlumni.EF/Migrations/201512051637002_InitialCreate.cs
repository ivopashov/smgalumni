namespace SmgAlumni.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsLetterCandidate", "SentOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsLetterCandidate", "SentOn");
        }
    }
}
