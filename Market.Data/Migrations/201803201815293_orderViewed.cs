namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderViewed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "BuyerChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "SellerChecked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "SellerChecked");
            DropColumn("dbo.Orders", "BuyerChecked");
        }
    }
}
