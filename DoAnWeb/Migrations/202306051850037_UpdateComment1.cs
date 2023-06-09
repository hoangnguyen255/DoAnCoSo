namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Comment", "isactive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Comment", "isactive");
        }
    }
}
