namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableComputedFields3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "WithmiddlemanSum", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "WithmiddlemanSum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
