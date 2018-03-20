namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccInfos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInfos",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        AdditionalInformation = c.String(),
                        BuyerId = c.String(maxLength: 128),
                        ModeratorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.BuyerId)
                .ForeignKey("dbo.UserProfiles", t => t.ModeratorId)
                .ForeignKey("dbo.Orders", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.BuyerId)
                .Index(t => t.ModeratorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountInfos", "Id", "dbo.Orders");
            DropForeignKey("dbo.AccountInfos", "ModeratorId", "dbo.UserProfiles");
            DropForeignKey("dbo.AccountInfos", "BuyerId", "dbo.UserProfiles");
            DropIndex("dbo.AccountInfos", new[] { "ModeratorId" });
            DropIndex("dbo.AccountInfos", new[] { "BuyerId" });
            DropIndex("dbo.AccountInfos", new[] { "Id" });
            DropTable("dbo.AccountInfos");
        }
    }
}
