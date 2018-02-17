namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilterItems", "Image", c => c.String());
            AddColumn("dbo.Games", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Image");
            DropColumn("dbo.FilterItems", "Image");
        }
    }
}
