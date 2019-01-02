namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Website_to_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WebsiteAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WebsiteAddress");
        }
    }
}
