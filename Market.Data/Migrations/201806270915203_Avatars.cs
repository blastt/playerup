namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Avatars : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Avatar32Path", c => c.String());
            AddColumn("dbo.UserProfiles", "Avatar48Path", c => c.String());
            AddColumn("dbo.UserProfiles", "Avatar96Path", c => c.String());
            DropColumn("dbo.UserProfiles", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "ImagePath", c => c.String());
            DropColumn("dbo.UserProfiles", "Avatar96Path");
            DropColumn("dbo.UserProfiles", "Avatar48Path");
            DropColumn("dbo.UserProfiles", "Avatar32Path");
        }
    }
}
