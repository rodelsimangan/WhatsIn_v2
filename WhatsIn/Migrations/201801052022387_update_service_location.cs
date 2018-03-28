namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_service_location : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Location", c => c.String());
            DropColumn("dbo.Services", "City");
            DropColumn("dbo.Services", "Province");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "Province", c => c.String());
            AddColumn("dbo.Services", "City", c => c.String());
            DropColumn("dbo.Services", "Location");
        }
    }
}
