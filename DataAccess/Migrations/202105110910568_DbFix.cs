namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inspections", "CustomerId", "dbo.Companies");
            DropIndex("dbo.Inspections", new[] { "CustomerId" });
            DropColumn("dbo.Inspections", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inspections", "CustomerId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Inspections", "CustomerId");
            AddForeignKey("dbo.Inspections", "CustomerId", "dbo.Companies", "Id");
        }
    }
}
