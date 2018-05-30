namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LockoutReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "LockoutReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "LockoutReason");
        }
    }
}
