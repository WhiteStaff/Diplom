namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInspectionStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inspections", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inspections", "Status");
        }
    }
}
