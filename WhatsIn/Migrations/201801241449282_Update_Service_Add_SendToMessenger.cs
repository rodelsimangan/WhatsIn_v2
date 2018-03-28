namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Service_Add_SendToMessenger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "SendToMessenger", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "SendToMessenger");
        }
    }
}
