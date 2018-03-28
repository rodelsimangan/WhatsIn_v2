namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Update_Ratings_Add_ServiceId_Rating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Ratings", "Rating", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ratings", "UserId");
            DropColumn("dbo.Ratings", "IsPositive");
            DropColumn("dbo.Ratings", "IsNegative");

        }

        public override void Down()
        {
            AddColumn("dbo.Ratings", "IsNegative", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ratings", "IsPositive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ratings", "UserId", c => c.String());
            DropColumn("dbo.Ratings", "Rating");
            DropColumn("dbo.Ratings", "ServiceId");
        }
    }
}
