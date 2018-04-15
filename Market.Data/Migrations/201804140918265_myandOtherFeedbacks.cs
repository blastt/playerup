namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class myandOtherFeedbacks : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Feedbacks", name: "UserId", newName: "UserToId");
            RenameIndex(table: "dbo.Feedbacks", name: "IX_UserId", newName: "IX_UserToId");
            AddColumn("dbo.Feedbacks", "UserFromId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Feedbacks", "UserFromId");
            AddForeignKey("dbo.Feedbacks", "UserFromId", "dbo.UserProfiles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "UserFromId", "dbo.UserProfiles");
            DropIndex("dbo.Feedbacks", new[] { "UserFromId" });
            DropColumn("dbo.Feedbacks", "UserFromId");
            RenameIndex(table: "dbo.Feedbacks", name: "IX_UserToId", newName: "IX_UserId");
            RenameColumn(table: "dbo.Feedbacks", name: "UserToId", newName: "UserId");
        }
    }
}
