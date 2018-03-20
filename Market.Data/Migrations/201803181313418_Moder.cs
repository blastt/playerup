namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Moder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "SellerId", "dbo.UserProfiles");
            DropIndex("dbo.Orders", new[] { "SellerId" });
            AddColumn("dbo.Orders", "ModeratorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Orders", "SellerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ModeratorId");
            CreateIndex("dbo.Orders", "SellerId");
            AddForeignKey("dbo.Orders", "ModeratorId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Orders", "SellerId", "dbo.UserProfiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "SellerId", "dbo.UserProfiles");
            DropForeignKey("dbo.Orders", "ModeratorId", "dbo.UserProfiles");
            DropIndex("dbo.Orders", new[] { "SellerId" });
            DropIndex("dbo.Orders", new[] { "ModeratorId" });
            AlterColumn("dbo.Orders", "SellerId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Orders", "ModeratorId");
            CreateIndex("dbo.Orders", "SellerId");
            AddForeignKey("dbo.Orders", "SellerId", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
    }
}
