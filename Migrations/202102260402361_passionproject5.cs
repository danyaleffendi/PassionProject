namespace PassionProject_Danyal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passionproject5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schedules", "DriverID", "dbo.Drivers");
            AddColumn("dbo.Drivers", "Schedule_RaceID", c => c.Int());
            AddColumn("dbo.Schedules", "Driver_DriverID", c => c.Int());
            CreateIndex("dbo.Drivers", "Schedule_RaceID");
            CreateIndex("dbo.Schedules", "Driver_DriverID");
            AddForeignKey("dbo.Drivers", "Schedule_RaceID", "dbo.Schedules", "RaceID");
            AddForeignKey("dbo.Schedules", "Driver_DriverID", "dbo.Drivers", "DriverID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schedules", "Driver_DriverID", "dbo.Drivers");
            DropForeignKey("dbo.Drivers", "Schedule_RaceID", "dbo.Schedules");
            DropIndex("dbo.Schedules", new[] { "Driver_DriverID" });
            DropIndex("dbo.Drivers", new[] { "Schedule_RaceID" });
            DropColumn("dbo.Schedules", "Driver_DriverID");
            DropColumn("dbo.Drivers", "Schedule_RaceID");
            AddForeignKey("dbo.Schedules", "DriverID", "dbo.Drivers", "DriverID", cascadeDelete: true);
        }
    }
}
