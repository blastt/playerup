namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transactions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BuyerInvoices", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.BuyerInvoices", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.SellerInvoices", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.SellerInvoices", "UserId", "dbo.UserProfiles");
            DropIndex("dbo.BuyerInvoices", new[] { "UserId" });
            DropIndex("dbo.BuyerInvoices", new[] { "OrderId" });
            DropIndex("dbo.SellerInvoices", new[] { "UserId" });
            DropIndex("dbo.SellerInvoices", new[] { "OrderId" });
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.UserProfiles", t => t.ReceiverId)
                .ForeignKey("dbo.UserProfiles", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.OrderId);
            
            DropTable("dbo.BuyerInvoices");
            DropTable("dbo.SellerInvoices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SellerInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DatePay = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyerInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DatePay = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Transactions", "SenderId", "dbo.UserProfiles");
            DropForeignKey("dbo.Transactions", "ReceiverId", "dbo.UserProfiles");
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            DropIndex("dbo.Transactions", new[] { "OrderId" });
            DropIndex("dbo.Transactions", new[] { "ReceiverId" });
            DropIndex("dbo.Transactions", new[] { "SenderId" });
            DropTable("dbo.Transactions");
            CreateIndex("dbo.SellerInvoices", "OrderId");
            CreateIndex("dbo.SellerInvoices", "UserId");
            CreateIndex("dbo.BuyerInvoices", "OrderId");
            CreateIndex("dbo.BuyerInvoices", "UserId");
            AddForeignKey("dbo.SellerInvoices", "UserId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SellerInvoices", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BuyerInvoices", "UserId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BuyerInvoices", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
