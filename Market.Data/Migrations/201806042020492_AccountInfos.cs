namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountInfos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountInfos", "Id", "dbo.Orders");
            DropIndex("dbo.AccountInfos", new[] { "Id" });
            DropPrimaryKey("dbo.AccountInfos");
            AddColumn("dbo.AccountInfos", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.AccountInfos", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.AccountInfos", "Id");
            CreateIndex("dbo.AccountInfos", "OrderId");
            AddForeignKey("dbo.AccountInfos", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountInfos", "OrderId", "dbo.Orders");
            DropIndex("dbo.AccountInfos", new[] { "OrderId" });
            DropPrimaryKey("dbo.AccountInfos");
            AlterColumn("dbo.AccountInfos", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.AccountInfos", "OrderId");
            AddPrimaryKey("dbo.AccountInfos", "Id");
            CreateIndex("dbo.AccountInfos", "Id");
            AddForeignKey("dbo.AccountInfos", "Id", "dbo.Orders", "Id");
        }
    }
}
