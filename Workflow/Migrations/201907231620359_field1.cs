namespace Workflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class field1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Subject", c => c.String());
            AddColumn("dbo.Vacations", "Subject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacations", "Subject");
            DropColumn("dbo.Accounts", "Subject");
        }
    }
}
