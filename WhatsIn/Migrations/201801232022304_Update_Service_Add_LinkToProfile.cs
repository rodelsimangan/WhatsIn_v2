namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Service_Add_LinkToProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "LinkToProfile", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "LinkToProfile");
        }
    }
}
