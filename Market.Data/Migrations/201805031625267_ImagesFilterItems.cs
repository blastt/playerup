namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagesFilterItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FilterItems", "ImageData", c => c.Binary());
            AddColumn("dbo.FilterItems", "ImageMimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FilterItems", "ImageMimeType");
            DropColumn("dbo.FilterItems", "ImageData");
        }
    }
}
