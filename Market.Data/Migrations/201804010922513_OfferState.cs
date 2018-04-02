namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "State");
        }
    }
}
