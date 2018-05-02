namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailPAssword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfos", "EmailPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfos", "EmailPassword");
        }
    }
}
