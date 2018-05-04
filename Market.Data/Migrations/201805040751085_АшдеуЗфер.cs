namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class АшдеуЗфер : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "ImagePath", c => c.String());
            DropColumn("dbo.Games", "ImageData");
            DropColumn("dbo.Games", "ImageMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "ImageMimeType", c => c.String());
            AddColumn("dbo.Games", "ImageData", c => c.Binary());
            DropColumn("dbo.Games", "ImagePath");
        }
    }
}
