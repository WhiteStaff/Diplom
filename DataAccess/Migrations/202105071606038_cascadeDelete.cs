namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            AddForeignKey("dbo.Employees", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
