namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CareersVolunteers : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Careers", "department_id");
            CreateIndex("dbo.Volunteers", "program_id");
            AddForeignKey("dbo.Careers", "department_id", "dbo.Departments", "department_id", cascadeDelete: true);
            AddForeignKey("dbo.Volunteers", "program_id", "dbo.Programs", "program_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "program_id", "dbo.Programs");
            DropForeignKey("dbo.Careers", "department_id", "dbo.Departments");
            DropIndex("dbo.Volunteers", new[] { "program_id" });
            DropIndex("dbo.Careers", new[] { "department_id" });
        }
    }
}
