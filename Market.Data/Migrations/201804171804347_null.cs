namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "Price", c => c.Decimal(nullable: false, precision: 8, scale: 2));
        }
    }
}
