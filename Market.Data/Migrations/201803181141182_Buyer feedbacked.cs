namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Buyerfeedbacked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "BuyerFeedbacked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "SellerFeedbacked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "IsFeedbacked");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "IsFeedbacked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "SellerFeedbacked");
            DropColumn("dbo.Orders", "BuyerFeedbacked");
        }
    }
}
