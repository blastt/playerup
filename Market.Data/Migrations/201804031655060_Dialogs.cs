namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dialogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dialogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User1Id = c.String(maxLength: 128),
                        User2Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.User1Id)
                .ForeignKey("dbo.UserProfiles", t => t.User2Id)
                .Index(t => t.User1Id)
                .Index(t => t.User2Id);
            
            AddColumn("dbo.Messages", "DialogId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "DialogId");
            AddForeignKey("dbo.Messages", "DialogId", "dbo.Dialogs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dialogs", "User2Id", "dbo.UserProfiles");
            DropForeignKey("dbo.Dialogs", "User1Id", "dbo.UserProfiles");
            DropForeignKey("dbo.Messages", "DialogId", "dbo.Dialogs");
            DropIndex("dbo.Messages", new[] { "DialogId" });
            DropIndex("dbo.Dialogs", new[] { "User2Id" });
            DropIndex("dbo.Dialogs", new[] { "User1Id" });
            DropColumn("dbo.Messages", "DialogId");
            DropTable("dbo.Dialogs");
        }
    }
}
