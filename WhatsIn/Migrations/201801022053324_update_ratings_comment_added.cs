namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_ratings_comment_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "Comment");
        }
    }
}
