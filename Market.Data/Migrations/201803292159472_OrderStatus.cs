namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderStatusOrders",
                c => new
                    {
                        OrderStatus_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderStatus_Id, t.Order_Id })
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id, cascadeDelete: true)
                .Index(t => t.OrderStatus_Id)
                .Index(t => t.Order_Id);
            
            DropColumn("dbo.Orders", "OrderStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderStatusOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderStatusOrders", "OrderStatus_Id", "dbo.OrderStatus");
            DropIndex("dbo.OrderStatusOrders", new[] { "Order_Id" });
            DropIndex("dbo.OrderStatusOrders", new[] { "OrderStatus_Id" });
            DropTable("dbo.OrderStatusOrders");
            DropTable("dbo.OrderStatus");
        }
    }
}
