namespace StockTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class openprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "OpenPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "OpenPrice");
        }
    }
}
