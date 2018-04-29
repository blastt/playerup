namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableComputedFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "Rating", c => c.Int());
            AlterColumn("dbo.UserProfiles", "AllFeedbackCount", c => c.Int());
            AlterColumn("dbo.UserProfiles", "PositiveFeedbackProcent", c => c.Double());
            AlterColumn("dbo.UserProfiles", "NegativeFeedbackProcent", c => c.Double());
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "MiddlemanPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.UserProfiles", "NegativeFeedbackProcent", c => c.Double(nullable: false));
            AlterColumn("dbo.UserProfiles", "PositiveFeedbackProcent", c => c.Double(nullable: false));
            AlterColumn("dbo.UserProfiles", "AllFeedbackCount", c => c.Int(nullable: false));
            AlterColumn("dbo.UserProfiles", "Rating", c => c.Int(nullable: false));
        }
    }
}
