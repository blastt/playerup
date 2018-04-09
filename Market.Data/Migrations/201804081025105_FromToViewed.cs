namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FromToViewed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "FromViewed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "ToViewed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Messages", "IsViewed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "IsViewed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Messages", "ToViewed");
            DropColumn("dbo.Messages", "FromViewed");
        }
    }
}
