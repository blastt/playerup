namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Name : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfileDialogs", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.UserProfileDialogs", "Dialog_Id", "dbo.Dialogs");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Feedbacks", "ReceiverId", "dbo.UserProfiles");
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Feedbacks", new[] { "SenderId" });
            DropIndex("dbo.Orders", new[] { "SellerId" });
            DropIndex("dbo.UserProfileDialogs", new[] { "UserProfile_Id" });
            DropIndex("dbo.UserProfileDialogs", new[] { "Dialog_Id" });
            RenameColumn(table: "dbo.Feedbacks", name: "SenderId", newName: "BuyerId");
            RenameColumn(table: "dbo.Feedbacks", name: "ReceiverId", newName: "SellerId");
            RenameColumn(table: "dbo.Orders", name: "ModeratorId", newName: "MiddlemanId");
            RenameIndex(table: "dbo.Feedbacks", name: "IX_ReceiverId", newName: "IX_SellerId");
            RenameIndex(table: "dbo.Orders", name: "IX_ModeratorId", newName: "IX_MiddlemanId");
            AddColumn("dbo.Dialogs", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Dialogs", "CompanionId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Messages", "SenderId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Feedbacks", "BuyerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "SellerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Dialogs", "CreatorId");
            CreateIndex("dbo.Dialogs", "CompanionId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Feedbacks", "BuyerId");
            CreateIndex("dbo.Orders", "SellerId");
            AddForeignKey("dbo.Dialogs", "CreatorId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Dialogs", "CompanionId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Feedbacks", "SellerId", "dbo.UserProfiles", "Id");
            DropTable("dbo.UserProfileDialogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProfileDialogs",
                c => new
                    {
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                        Dialog_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfile_Id, t.Dialog_Id });
            
            DropForeignKey("dbo.Feedbacks", "SellerId", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Dialogs", "CompanionId", "dbo.UserProfiles");
            DropForeignKey("dbo.Dialogs", "CreatorId", "dbo.UserProfiles");
            DropIndex("dbo.Orders", new[] { "SellerId" });
            DropIndex("dbo.Feedbacks", new[] { "BuyerId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Dialogs", new[] { "CompanionId" });
            DropIndex("dbo.Dialogs", new[] { "CreatorId" });
            AlterColumn("dbo.Orders", "SellerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Feedbacks", "BuyerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Messages", "SenderId", c => c.String(maxLength: 128));
            DropColumn("dbo.Dialogs", "CompanionId");
            DropColumn("dbo.Dialogs", "CreatorId");
            RenameIndex(table: "dbo.Orders", name: "IX_MiddlemanId", newName: "IX_ModeratorId");
            RenameIndex(table: "dbo.Feedbacks", name: "IX_SellerId", newName: "IX_ReceiverId");
            RenameColumn(table: "dbo.Orders", name: "MiddlemanId", newName: "ModeratorId");
            RenameColumn(table: "dbo.Feedbacks", name: "SellerId", newName: "ReceiverId");
            RenameColumn(table: "dbo.Feedbacks", name: "BuyerId", newName: "SenderId");
            CreateIndex("dbo.UserProfileDialogs", "Dialog_Id");
            CreateIndex("dbo.UserProfileDialogs", "UserProfile_Id");
            CreateIndex("dbo.Orders", "SellerId");
            CreateIndex("dbo.Feedbacks", "SenderId");
            CreateIndex("dbo.Messages", "SenderId");
            AddForeignKey("dbo.Feedbacks", "ReceiverId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "ReceiverId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserProfileDialogs", "Dialog_Id", "dbo.Dialogs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserProfileDialogs", "UserProfile_Id", "dbo.UserProfiles", "Id", cascadeDelete: true);
        }
    }
}
