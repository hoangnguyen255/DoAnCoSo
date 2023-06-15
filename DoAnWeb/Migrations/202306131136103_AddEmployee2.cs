namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployee2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "diachi", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "diachi");
        }
    }
}
