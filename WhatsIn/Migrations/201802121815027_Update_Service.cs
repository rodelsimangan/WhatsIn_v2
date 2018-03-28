namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Service : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Type", c => c.String());
            AddColumn("dbo.Services", "ServiceProvides", c => c.String());
            AddColumn("dbo.Services", "PhoneNumber", c => c.String());
            AddColumn("dbo.Services", "Schedule", c => c.String());
            AddColumn("dbo.Services", "FacebookPage", c => c.String());
            AddColumn("dbo.Services", "TwitterPage", c => c.String());
            AddColumn("dbo.Services", "InstagramPage", c => c.String());
            AddColumn("dbo.Services", "WebsiteAddress", c => c.String());
            DropColumn("dbo.Services", "Description");
            DropColumn("dbo.Services", "LinkToProfile");
            DropColumn("dbo.Services", "SendToMessenger");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "SendToMessenger", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "LinkToProfile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Services", "Description", c => c.String());
            DropColumn("dbo.Services", "WebsiteAddress");
            DropColumn("dbo.Services", "InstagramPage");
            DropColumn("dbo.Services", "TwitterPage");
            DropColumn("dbo.Services", "FacebookPage");
            DropColumn("dbo.Services", "Schedule");
            DropColumn("dbo.Services", "PhoneNumber");
            DropColumn("dbo.Services", "ServiceProvides");
            DropColumn("dbo.Services", "Type");
        }
    }
}
