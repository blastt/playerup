namespace Market.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters");
            DropIndex("dbo.FilterItems", new[] { "Filter_Id" });
            AlterColumn("dbo.FilterItems", "Filter_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.FilterItems", "Filter_Id");
            AddForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters");
            DropIndex("dbo.FilterItems", new[] { "Filter_Id" });
            AlterColumn("dbo.FilterItems", "Filter_Id", c => c.Int());
            CreateIndex("dbo.FilterItems", "Filter_Id");
            AddForeignKey("dbo.FilterItems", "Filter_Id", "dbo.Filters", "Id");
        }
    }
}
