namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Role = c.Int(nullable: false),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CompanyId = c.Guid(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .Index(t => t.InspectionId);
            
            CreateTable(
                "dbo.InspectionEmployees",
                c => new
                    {
                        Inspection_Id = c.Guid(nullable: false),
                        Employee_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Inspection_Id, t.Employee_Id })
                .ForeignKey("dbo.Inspections", t => t.Inspection_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Inspection_Id)
                .Index(t => t.Employee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Inspections", "CustomerId", "dbo.Companies");
            DropForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropIndex("dbo.InspectionEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.InspectionEmployees", new[] { "Inspection_Id" });
            DropIndex("dbo.Events", new[] { "InspectionId" });
            DropIndex("dbo.Inspections", new[] { "CustomerId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropTable("dbo.InspectionEmployees");
            DropTable("dbo.Events");
            DropTable("dbo.Inspections");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
        }
    }
}
