namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.table_Comment", "message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.table_Comment", "message");
        }
    }
}
