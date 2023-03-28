namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAQ : DbMigration
    {
        public override void Up()
        {   
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        FAQ_Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FAQ_Id)
                .ForeignKey("dbo.Departments", t => t.department_Id, cascadeDelete: true)
                .Index(t => t.department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FAQs", "department_Id", "dbo.Departments");
            DropIndex("dbo.FAQs", new[] { "department_Id" });
            DropTable("dbo.FAQs");
       
        }
    }
}
