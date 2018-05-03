namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Billings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Billings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCeated = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OrderId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SellerInvoices", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.SellerInvoices", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.BuyerInvoices", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.BuyerInvoices", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Billings", "UserId", "dbo.UserProfiles");
            DropIndex("dbo.SellerInvoices", new[] { "OrderId" });
            DropIndex("dbo.SellerInvoices", new[] { "UserId" });
            DropIndex("dbo.BuyerInvoices", new[] { "OrderId" });
            DropIndex("dbo.BuyerInvoices", new[] { "UserId" });
            DropIndex("dbo.Billings", new[] { "UserId" });
            DropTable("dbo.SellerInvoices");
            DropTable("dbo.BuyerInvoices");
            DropTable("dbo.Billings");
        }
    }
}
