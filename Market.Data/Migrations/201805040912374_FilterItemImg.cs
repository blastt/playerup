namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilterItemImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilterItems", "ImagePath", c => c.String());
            DropColumn("dbo.FilterItems", "ImageData");
            DropColumn("dbo.FilterItems", "ImageMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FilterItems", "ImageMimeType", c => c.String());
            AddColumn("dbo.FilterItems", "ImageData", c => c.Binary());
            DropColumn("dbo.FilterItems", "ImagePath");
        }
    }
}
