namespace PassionProject_Danyal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passionprojectdanyal2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PSNTag = c.String(),
                        Nationality = c.String(),
                        Abbreviation = c.String(),
                        Status = c.String(),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DriverID)
                .ForeignKey("dbo.Teams", t => t.TeamID, cascadeDelete: true)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        RaceID = c.Int(nullable: false, identity: true),
                        Round = c.Int(nullable: false),
                        Circuit = c.String(),
                        Date = c.String(),
                        DriverID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RaceID)
                .ForeignKey("dbo.Drivers", t => t.DriverID, cascadeDelete: true)
                .Index(t => t.DriverID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        TeamColor = c.String(),
                    })
                .PrimaryKey(t => t.TeamID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "TeamID", "dbo.Teams");
            DropForeignKey("dbo.Schedules", "DriverID", "dbo.Drivers");
            DropIndex("dbo.Schedules", new[] { "DriverID" });
            DropIndex("dbo.Drivers", new[] { "TeamID" });
            DropTable("dbo.Teams");
            DropTable("dbo.Schedules");
            DropTable("dbo.Drivers");
        }
    }
}
