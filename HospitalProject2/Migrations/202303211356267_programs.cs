namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Programs",
                c => new
                    {
                        program_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        department_id = c.Int(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.program_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Programs");
        }
    }
}
