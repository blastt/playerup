namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Sum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "Amount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Orders", "WithdrawAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "WithdrawAmount");
            DropColumn("dbo.Orders", "Amount");
            DropColumn("dbo.Orders", "Sum");
            DropColumn("dbo.UserProfiles", "Balance");
        }
    }
}
