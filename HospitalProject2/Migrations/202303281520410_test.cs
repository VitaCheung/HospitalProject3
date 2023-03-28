namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                {
                    donations_id = c.Int(nullable: false, identity: true),
                    department_id = c.Int(nullable: false),
                    name = c.String(),
                    email = c.String(),
                    amount = c.Decimal(nullable: false),
                })
                .PrimaryKey(t => t.donations_id);

        }

        public override void Down()
        {
            DropTable("dbo.Donations");
        }
    }
}
