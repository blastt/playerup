namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Middleman : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "SellerPaysMiddleman", c => c.Boolean(nullable: false));
            AddColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "MiddlemanPrice");
            DropColumn("dbo.Offers", "SellerPaysMiddleman");
        }
    }
}
