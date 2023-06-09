namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Table", "quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Table", "quantity");
        }
    }
}
