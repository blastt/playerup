namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SumToSeller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "AmmountSellerGet", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Orders", "WithdrawAmountSellerGet", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "WithdrawAmountSellerGet");
            DropColumn("dbo.Orders", "AmmountSellerGet");
        }
    }
}
