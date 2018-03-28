namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_application_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        LoginId = c.String(),
                        IsPositive = c.Boolean(),
                        IsNegative = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Watchlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.String(),
                        LoginId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Watchlists");
            DropTable("dbo.Services");
            DropTable("dbo.ServiceImages");
            DropTable("dbo.Ratings");
        }
    }
}
