namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class staffs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        staff_id = c.Int(nullable: false, identity: true),
                        f_name = c.String(),
                        l_name = c.String(),
                        department_id = c.Int(nullable: false),
                        bio = c.String(),
                        image = c.String(),
                    })
                .PrimaryKey(t => t.staff_id)
                .ForeignKey("dbo.Departments", t => t.department_id, cascadeDelete: true)
                .Index(t => t.department_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "department_id", "dbo.Departments");
            DropIndex("dbo.Staffs", new[] { "department_id" });
            DropTable("dbo.Staffs");
        }
    }
}
