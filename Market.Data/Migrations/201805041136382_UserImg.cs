namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserImg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "ImagePath", c => c.String());
            DropColumn("dbo.UserProfiles", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "Avatar", c => c.Binary());
            DropColumn("dbo.UserProfiles", "ImagePath");
        }
    }
}
