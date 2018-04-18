namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithmiddlemanSum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "WithmiddlemanSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "WithmiddlemanSum");
        }
    }
}
