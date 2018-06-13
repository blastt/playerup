namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScreenModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScreenshotPathes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        OfferId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.OfferId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScreenshotPathes", "OfferId", "dbo.Offers");
            DropIndex("dbo.ScreenshotPathes", new[] { "OfferId" });
            DropTable("dbo.ScreenshotPathes");
        }
    }
}
