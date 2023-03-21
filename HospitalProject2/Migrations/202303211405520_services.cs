namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class services : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        service_id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        program_id = c.Int(nullable: false),
                        description = c.String(),
                        location = c.String(),
                    })
                .PrimaryKey(t => t.service_id)
                .ForeignKey("dbo.Programs", t => t.program_id, cascadeDelete: true)
                .Index(t => t.program_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "program_id", "dbo.Programs");
            DropIndex("dbo.Services", new[] { "program_id" });
            DropTable("dbo.Services");
        }
    }
}
