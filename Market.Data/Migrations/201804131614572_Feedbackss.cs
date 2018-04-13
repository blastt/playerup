namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feedbackss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedbacks", "SellerId", "dbo.UserProfiles");
            DropIndex("dbo.Feedbacks", new[] { "SellerId" });
            RenameColumn(table: "dbo.Feedbacks", name: "BuyerId", newName: "UserId");
            RenameIndex(table: "dbo.Feedbacks", name: "IX_BuyerId", newName: "IX_UserId");
            DropColumn("dbo.Feedbacks", "SellerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedbacks", "SellerId", c => c.String(nullable: false, maxLength: 128));
            RenameIndex(table: "dbo.Feedbacks", name: "IX_UserId", newName: "IX_BuyerId");
            RenameColumn(table: "dbo.Feedbacks", name: "UserId", newName: "BuyerId");
            CreateIndex("dbo.Feedbacks", "SellerId");
            AddForeignKey("dbo.Feedbacks", "SellerId", "dbo.UserProfiles", "Id");
        }
    }
}
