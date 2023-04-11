namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Donations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                {
                    donation_id = c.Int(nullable: false, identity: true),
                    department_id = c.Int(nullable: false),
                    name = c.String(),
                    email = c.String(),
                    amount = c.Decimal(nullable: false),
                })
                .PrimaryKey(t => t.donation_id);

        }

        public override void Down()
        {
            DropTable("dbo.Donations");
        }
    }
}
