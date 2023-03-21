namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class programsdepartments : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Programs", "department_id");
            AddForeignKey("dbo.Programs", "department_id", "dbo.Departments", "department_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Programs", "department_id", "dbo.Departments");
            DropIndex("dbo.Programs", new[] { "department_id" });
        }
    }
}
