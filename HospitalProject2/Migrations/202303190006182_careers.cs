namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class careers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        job_id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        department_id = c.Int(nullable: false),
                        category = c.String(),
                        job_type = c.String(),
                        posting_date = c.DateTime(nullable: false),
                        closing_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.job_id);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        volunteer_id = c.Int(nullable: false, identity: true),
                        f_name = c.String(),
                        l_name = c.String(),
                        contact = c.Int(nullable: false),
                        email = c.String(),
                        program_id = c.Int(nullable: false),
                        hours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.volunteer_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Volunteers");
            DropTable("dbo.Careers");
        }
    }
}
