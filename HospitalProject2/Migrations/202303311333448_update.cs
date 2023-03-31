namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "department_id", "dbo.Departments");
            DropIndex("dbo.Donations", new[] { "department_id" });
            DropTable("dbo.Donations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        donation_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        email = c.String(),
                        department_id = c.Int(nullable: false),
                        phone = c.String(),
                        amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.donation_id);
            
            CreateIndex("dbo.Donations", "department_id");
            AddForeignKey("dbo.Donations", "department_id", "dbo.Departments", "department_id", cascadeDelete: true);
        }
    }
}
