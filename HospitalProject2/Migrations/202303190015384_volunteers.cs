namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class volunteers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Volunteers", "contact", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Volunteers", "contact", c => c.Int(nullable: false));
        }
    }
}
