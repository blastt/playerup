namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DialogUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dialogs", "User1Id", "dbo.UserProfiles");
            DropForeignKey("dbo.Dialogs", "User2Id", "dbo.UserProfiles");
            DropIndex("dbo.Dialogs", new[] { "User1Id" });
            DropIndex("dbo.Dialogs", new[] { "User2Id" });
            CreateTable(
                "dbo.UserProfileDialogs",
                c => new
                    {
                        UserProfile_Id = c.String(nullable: false, maxLength: 128),
                        Dialog_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfile_Id, t.Dialog_Id })
                .ForeignKey("dbo.UserProfiles", t => t.UserProfile_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dialogs", t => t.Dialog_Id, cascadeDelete: true)
                .Index(t => t.UserProfile_Id)
                .Index(t => t.Dialog_Id);
            
            DropColumn("dbo.Dialogs", "User1Id");
            DropColumn("dbo.Dialogs", "User2Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dialogs", "User2Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Dialogs", "User1Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.UserProfileDialogs", "Dialog_Id", "dbo.Dialogs");
            DropForeignKey("dbo.UserProfileDialogs", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.UserProfileDialogs", new[] { "Dialog_Id" });
            DropIndex("dbo.UserProfileDialogs", new[] { "UserProfile_Id" });
            DropTable("dbo.UserProfileDialogs");
            CreateIndex("dbo.Dialogs", "User2Id");
            CreateIndex("dbo.Dialogs", "User1Id");
            AddForeignKey("dbo.Dialogs", "User2Id", "dbo.UserProfiles", "Id");
            AddForeignKey("dbo.Dialogs", "User1Id", "dbo.UserProfiles", "Id");
        }
    }
}
