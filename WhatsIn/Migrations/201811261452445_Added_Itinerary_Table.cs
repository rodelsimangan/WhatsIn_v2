namespace WhatsIn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Itinerary_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Itineraries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceId = c.String(),
                        LoginId = c.String(),
                        Sequence = c.Int(nullable: false),
                        ITDate = c.DateTime(),
                        IsDeleted = c.Boolean(),
                        IsCompleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Itineraries");
        }
    }
}
