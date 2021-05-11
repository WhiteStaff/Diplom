namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInspectionContractor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        ContractorId = c.Guid(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        FinalScore = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.ContractorId)
                .ForeignKey("dbo.Companies", t => t.CustomerId)
                .Index(t => t.CustomerId)
                .Index(t => t.ContractorId);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .Index(t => t.InspectionId);
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        InspectionId = c.Guid(nullable: false),
                        RequirementId = c.Int(nullable: false),
                        Score = c.Double(),
                    })
                .PrimaryKey(t => new { t.InspectionId, t.RequirementId })
                .ForeignKey("dbo.Inspections", t => t.InspectionId, cascadeDelete: true)
                .ForeignKey("dbo.Requirements", t => t.RequirementId, cascadeDelete: true)
                .Index(t => t.InspectionId)
                .Index(t => t.RequirementId);
            
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
            
            AddColumn("dbo.Events", "InspectionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Events", "InspectionId");
            AddForeignKey("dbo.Events", "InspectionId", "dbo.Inspections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Documents", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Inspections", "CustomerId", "dbo.Companies");
            DropForeignKey("dbo.Inspections", "ContractorId", "dbo.Companies");
            DropForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections");
            DropIndex("dbo.InspectionEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.InspectionEmployees", new[] { "Inspection_Id" });
            DropIndex("dbo.Events", new[] { "InspectionId" });
            DropIndex("dbo.Evaluations", new[] { "RequirementId" });
            DropIndex("dbo.Evaluations", new[] { "InspectionId" });
            DropIndex("dbo.Documents", new[] { "InspectionId" });
            DropIndex("dbo.Inspections", new[] { "ContractorId" });
            DropIndex("dbo.Inspections", new[] { "CustomerId" });
            DropColumn("dbo.Events", "InspectionId");
            DropTable("dbo.InspectionEmployees");
            DropTable("dbo.Evaluations");
            DropTable("dbo.Documents");
            DropTable("dbo.Inspections");
        }
    }
}
