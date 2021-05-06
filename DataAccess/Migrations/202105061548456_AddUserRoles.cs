namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Role");
        }
    }
}
