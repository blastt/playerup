namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderStatus1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderStatusOrders", "OrderStatus_Id", "dbo.OrderStatus");
            DropForeignKey("dbo.OrderStatusOrders", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderStatusOrders", new[] { "OrderStatus_Id" });
            DropIndex("dbo.OrderStatusOrders", new[] { "Order_Id" });
            AddColumn("dbo.OrderStatus", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderStatus", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Offers", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "DateCreated", c => c.DateTime(nullable: false));
            CreateIndex("dbo.OrderStatus", "OrderId");
            AddForeignKey("dbo.OrderStatus", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            DropTable("dbo.OrderStatusOrders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderStatusOrders",
                c => new
                    {
                        OrderStatus_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderStatus_Id, t.Order_Id });
            
            DropForeignKey("dbo.OrderStatus", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderStatus", new[] { "OrderId" });
            AlterColumn("dbo.Orders", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Offers", "DateCreated", c => c.DateTime());
            DropColumn("dbo.OrderStatus", "OrderId");
            DropColumn("dbo.OrderStatus", "DateCreated");
            CreateIndex("dbo.OrderStatusOrders", "Order_Id");
            CreateIndex("dbo.OrderStatusOrders", "OrderStatus_Id");
            AddForeignKey("dbo.OrderStatusOrders", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderStatusOrders", "OrderStatus_Id", "dbo.OrderStatus", "Id", cascadeDelete: true);
        }
    }
}
