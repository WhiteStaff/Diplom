namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNullability : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inspections", "StartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inspections", "StartDate", c => c.DateTime(nullable: false));
        }
    }
}
