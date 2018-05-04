namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImagesFilterItems2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FilterItems", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FilterItems", "Image", c => c.String());
        }
    }
}
