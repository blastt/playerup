namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobIdOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "JobId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "JobId");
        }
    }
}
