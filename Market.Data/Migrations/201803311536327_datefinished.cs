namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datefinished : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderStatus", "FinisedName", c => c.String());
            AddColumn("dbo.OrderStatus", "DateFinished", c => c.DateTime());
            DropColumn("dbo.OrderStatus", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderStatus", "DateCreated", c => c.DateTime(nullable: false));
            DropColumn("dbo.OrderStatus", "DateFinished");
            DropColumn("dbo.OrderStatus", "FinisedName");
        }
    }
}
