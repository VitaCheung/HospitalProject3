namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        patient_id = c.Int(nullable: false, identity: true),
                        health_num = c.Int(nullable: false),
                        f_name = c.String(),
                        l_name = c.String(),
                        bday = c.DateTime(nullable: false),
                        address = c.String(),
                        phone = c.String(),
                    })
                .PrimaryKey(t => t.patient_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Patients");
        }
    }
}
