namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderFeedbacks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedbacks", "Id", "dbo.Orders");
            DropIndex("dbo.Feedbacks", new[] { "Id" });
            DropPrimaryKey("dbo.Feedbacks");
            AddColumn("dbo.Feedbacks", "Order_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Feedbacks", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Feedbacks", "Id");
            CreateIndex("dbo.Feedbacks", "Order_Id");
            AddForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Feedbacks", new[] { "Order_Id" });
            DropPrimaryKey("dbo.Feedbacks");
            AlterColumn("dbo.Feedbacks", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Feedbacks", "Order_Id");
            AddPrimaryKey("dbo.Feedbacks", "Id");
            CreateIndex("dbo.Feedbacks", "Id");
            AddForeignKey("dbo.Feedbacks", "Id", "dbo.Orders", "Id");
        }
    }
}
