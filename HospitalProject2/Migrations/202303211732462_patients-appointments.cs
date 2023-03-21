namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patientsappointments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "patient_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "patient_id");
            AddForeignKey("dbo.Appointments", "patient_id", "dbo.Patients", "patient_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "patient_id", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "patient_id" });
            DropColumn("dbo.Appointments", "patient_id");
        }
    }
}
