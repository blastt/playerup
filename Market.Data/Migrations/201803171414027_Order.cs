namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "UserProfileId", newName: "SellerId");
            RenameIndex(table: "dbo.Orders", name: "IX_UserProfileId", newName: "IX_SellerId");
            AddColumn("dbo.Orders", "BuyerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "BuyerId");
            AddForeignKey("dbo.Orders", "BuyerId", "dbo.UserProfiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "BuyerId", "dbo.UserProfiles");
            DropIndex("dbo.Orders", new[] { "BuyerId" });
            DropColumn("dbo.Orders", "BuyerId");
            RenameIndex(table: "dbo.Orders", name: "IX_SellerId", newName: "IX_UserProfileId");
            RenameColumn(table: "dbo.Orders", name: "SellerId", newName: "UserProfileId");
        }
    }
}
