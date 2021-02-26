namespace PassionProject_Danyal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passionproject4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Schedules", "Abbreviation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "Abbreviation", c => c.String());
        }
    }
}
