namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkedList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "PositiveFeedbackCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "NegativeFeedbackCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "AllFeedbackCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.UserProfiles", "Positive");
            DropColumn("dbo.UserProfiles", "Negative");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "Negative", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "Positive", c => c.Int(nullable: false));
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.UserProfiles", "AllFeedbackCount");
            DropColumn("dbo.UserProfiles", "NegativeFeedbackCount");
            DropColumn("dbo.UserProfiles", "PositiveFeedbackCount");
        }
    }
}
