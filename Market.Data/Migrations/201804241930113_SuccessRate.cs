namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuccessRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "SuccessOrderRate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "SuccessOrderRate");
        }
    }
}
