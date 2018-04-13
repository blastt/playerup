namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackTest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Feedbacks", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedbacks", "OrderId", c => c.Int(nullable: false));
        }
    }
}
