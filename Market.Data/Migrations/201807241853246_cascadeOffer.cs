namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadeOffer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "CurrentStatusId", "dbo.OrderStatuses");
            DropForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Id", "dbo.Offers");
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            AddForeignKey("dbo.Orders", "CurrentStatusId", "dbo.OrderStatuses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Id", "dbo.Offers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Id", "dbo.Offers");
            DropForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CurrentStatusId", "dbo.OrderStatuses");
            AddForeignKey("dbo.Transactions", "OrderId", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "Id", "dbo.Offers", "Id");
            AddForeignKey("dbo.Feedbacks", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "CurrentStatusId", "dbo.OrderStatuses", "Id");
        }
    }
}
