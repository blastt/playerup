namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Images : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "ImageData", c => c.Binary());
            AddColumn("dbo.Games", "ImageMimeType", c => c.String());
            DropColumn("dbo.Games", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Image", c => c.String());
            DropColumn("dbo.Games", "ImageMimeType");
            DropColumn("dbo.Games", "ImageData");
        }
    }
}
