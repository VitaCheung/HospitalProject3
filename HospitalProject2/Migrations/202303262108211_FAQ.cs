namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAQ : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        appointment_id = c.Int(nullable: false, identity: true),
                        health_num = c.Int(nullable: false),
                        date_time = c.DateTime(nullable: false),
                        symptoms = c.String(),
                        patient_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.appointment_id)
                .ForeignKey("dbo.Patients", t => t.patient_id, cascadeDelete: true)
                .Index(t => t.patient_id);
            
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
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        donation_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        department_Id = c.Int(nullable: false),
                        Phone = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.donation_Id)
                .ForeignKey("dbo.Departments", t => t.department_Id, cascadeDelete: true)
                .Index(t => t.department_Id);
            
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
            DropForeignKey("dbo.Donations", "department_Id", "dbo.Departments");
            DropForeignKey("dbo.Appointments", "patient_id", "dbo.Patients");
            DropIndex("dbo.FAQs", new[] { "department_Id" });
            DropIndex("dbo.Donations", new[] { "department_Id" });
            DropIndex("dbo.Appointments", new[] { "patient_id" });
            DropTable("dbo.FAQs");
            DropTable("dbo.Donations");
            DropTable("dbo.Patients");
            DropTable("dbo.Appointments");
        }
    }
}
