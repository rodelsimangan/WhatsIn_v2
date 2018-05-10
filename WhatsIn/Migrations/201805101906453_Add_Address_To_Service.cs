namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Address_To_Service : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Services", "Address");
        }
    }
}
