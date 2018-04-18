namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Calculated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "PositiveFeedbackProcent", c => c.Double(nullable: false));
            AddColumn("dbo.UserProfiles", "NegativeFeedbackProcent", c => c.Double(nullable: false));
            AlterColumn("dbo.UserProfiles", "Positive", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "Positive", c => c.Int(nullable: false));
            DropColumn("dbo.UserProfiles", "NegativeFeedbackProcent");
            DropColumn("dbo.UserProfiles", "PositiveFeedbackProcent");
            DropColumn("dbo.UserProfiles", "Rating");
        }
    }
}
