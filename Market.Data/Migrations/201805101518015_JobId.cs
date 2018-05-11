namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "JobId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "JobId");
        }
    }
}
