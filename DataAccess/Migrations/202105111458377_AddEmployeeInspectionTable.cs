namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeInspectionTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.InspectionEmployees", new[] { "Inspection_Id" });
            DropIndex("dbo.InspectionEmployees", new[] { "Employee_Id" });
            CreateTable(
                "dbo.EmployeeInspections",
                c => new
                    {
                        EmployeeId = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        Approved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.InspectionId })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.InspectionId);
            
            DropTable("dbo.InspectionEmployees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InspectionEmployees",
                c => new
                    {
                        Inspection_Id = c.Guid(nullable: false),
                        Employee_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Inspection_Id, t.Employee_Id });
            
            DropForeignKey("dbo.EmployeeInspections", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.EmployeeInspections", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.EmployeeInspections", new[] { "InspectionId" });
            DropIndex("dbo.EmployeeInspections", new[] { "EmployeeId" });
            DropTable("dbo.EmployeeInspections");
            CreateIndex("dbo.InspectionEmployees", "Employee_Id");
            CreateIndex("dbo.InspectionEmployees", "Inspection_Id");
            AddForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections", "Id", cascadeDelete: true);
        }
    }
}
