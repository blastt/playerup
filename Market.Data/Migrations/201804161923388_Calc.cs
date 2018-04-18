namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Calc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "PositiveFeedbackCount", c => c.Int(nullable: false));
            AlterColumn("dbo.UserProfiles", "NegativeFeedbackCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "NegativeFeedbackCount", c => c.Int(nullable: false));
            AlterColumn("dbo.UserProfiles", "PositiveFeedbackCount", c => c.Int(nullable: false));
        }
    }
}
