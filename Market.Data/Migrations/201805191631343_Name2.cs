namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Name2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserProfiles", "LastVisitDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "LastVisitDate", c => c.DateTime());
        }
    }
}
