namespace DoAnWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployee1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "createdby", c => c.String());
            AddColumn("dbo.Employees", "createddate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "modifierdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "modifierby", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "modifierby");
            DropColumn("dbo.Employees", "modifierdate");
            DropColumn("dbo.Employees", "createddate");
            DropColumn("dbo.Employees", "createdby");
        }
    }
}
