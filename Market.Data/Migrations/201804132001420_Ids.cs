namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ids : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountInfos", "BuyerId", "dbo.UserProfiles");
            DropForeignKey("dbo.AccountInfos", "ModeratorId", "dbo.UserProfiles");
            DropIndex("dbo.AccountInfos", new[] { "BuyerId" });
            DropIndex("dbo.AccountInfos", new[] { "ModeratorId" });
            DropColumn("dbo.AccountInfos", "BuyerId");
            DropColumn("dbo.AccountInfos", "ModeratorId");
            DropColumn("dbo.Orders", "OfferId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OfferId", c => c.Int(nullable: false));
            AddColumn("dbo.AccountInfos", "ModeratorId", c => c.String(maxLength: 128));
            AddColumn("dbo.AccountInfos", "BuyerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AccountInfos", "ModeratorId");
            CreateIndex("dbo.AccountInfos", "BuyerId");
            AddForeignKey("dbo.AccountInfos", "ModeratorId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.AccountInfos", "BuyerId", "dbo.UserProfiles", "Id");
        }
    }
}
