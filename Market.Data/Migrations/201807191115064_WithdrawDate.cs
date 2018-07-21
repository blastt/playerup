namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WithdrawDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Withdraws", "DateCrated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Withdraws", "DateCrated");
        }
    }
}
