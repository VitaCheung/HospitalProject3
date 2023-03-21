namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        department_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        location = c.String(),
                        size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.department_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Departments");
        }
    }
}
