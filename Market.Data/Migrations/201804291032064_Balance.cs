namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Balance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Balance");
        }
    }
}
