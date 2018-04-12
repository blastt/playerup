namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderFeedback : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Offers", name: "Game_Id", newName: "GameId");
            RenameIndex(table: "dbo.Offers", name: "IX_Game_Id", newName: "IX_GameId");
            DropPrimaryKey("dbo.Feedbacks");
            AddColumn("dbo.Feedbacks", "OrderId", c => c.Int(nullable: false));
            AddColumn("dbo.Offers", "AccountLogin", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Games", "Rank", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OfferId", c => c.Int(nullable: false));
            AlterColumn("dbo.Feedbacks", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Feedbacks", "DateLeft", c => c.DateTime());
            AddPrimaryKey("dbo.Feedbacks", "Id");
            CreateIndex("dbo.Feedbacks", "Id");
            AddForeignKey("dbo.Feedbacks", "Id", "dbo.Orders", "Id");
            DropColumn("dbo.UserProfiles", "Rating");
            DropColumn("dbo.UserProfiles", "Balance");
            DropColumn("dbo.Messages", "Subject");
            DropColumn("dbo.Messages", "ParentMessageId");
            DropColumn("dbo.Feedbacks", "OfferId");
            DropColumn("dbo.Feedbacks", "OfferHeader");
            DropColumn("dbo.Offers", "SteamLogin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "SteamLogin", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Feedbacks", "OfferHeader", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Feedbacks", "OfferId", c => c.String());
            AddColumn("dbo.Messages", "ParentMessageId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "Subject", c => c.String());
            AddColumn("dbo.UserProfiles", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.UserProfiles", "Rating", c => c.Int(nullable: false));
            DropForeignKey("dbo.Feedbacks", "Id", "dbo.Orders");
            DropIndex("dbo.Feedbacks", new[] { "Id" });
            DropPrimaryKey("dbo.Feedbacks");
            AlterColumn("dbo.Feedbacks", "DateLeft", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Feedbacks", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Orders", "OfferId");
            DropColumn("dbo.Games", "Rank");
            DropColumn("dbo.Offers", "AccountLogin");
            DropColumn("dbo.Feedbacks", "OrderId");
            AddPrimaryKey("dbo.Feedbacks", "Id");
            RenameIndex(table: "dbo.Offers", name: "IX_GameId", newName: "IX_Game_Id");
            RenameColumn(table: "dbo.Offers", name: "GameId", newName: "Game_Id");
        }
    }
}
