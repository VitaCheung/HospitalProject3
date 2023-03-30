namespace HospitalProject2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentsstaffs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "staff_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Appointments", "staff_id");
            AddForeignKey("dbo.Appointments", "staff_id", "dbo.Staffs", "staff_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "staff_id", "dbo.Staffs");
            DropIndex("dbo.Appointments", new[] { "staff_id" });
            DropColumn("dbo.Appointments", "staff_id");
        }
    }
}
