namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Order", "ship", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Order", "ship");
        }
    }
}
