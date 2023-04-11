namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donationsedits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        donation_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        department_id = c.Int(nullable: false),
                        email = c.String(),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.donation_id)
                .ForeignKey("dbo.Departments", t => t.department_id, cascadeDelete: true)
                .Index(t => t.department_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "department_id", "dbo.Departments");
            DropIndex("dbo.Donations", new[] { "department_id" });
            DropTable("dbo.Donations");
        }
    }
}
