namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class volunteersuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Volunteers", "UserID");
            AddForeignKey("dbo.Volunteers", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Volunteers", new[] { "UserID" });
            DropColumn("dbo.Volunteers", "UserID");
        }
    }
}
