namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropInspections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Documents", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections");
            DropForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.Events", "InspectionId", "dbo.Inspections");
            DropIndex("dbo.Documents", new[] { "InspectionId" });
            DropIndex("dbo.Evaluations", new[] { "InspectionId" });
            DropIndex("dbo.Evaluations", new[] { "RequirementId" });
            DropIndex("dbo.Events", new[] { "InspectionId" });
            DropIndex("dbo.InspectionEmployees", new[] { "Inspection_Id" });
            DropIndex("dbo.InspectionEmployees", new[] { "Employee_Id" });
            DropColumn("dbo.Events", "InspectionId");
            DropTable("dbo.Inspections");
            DropTable("dbo.Documents");
            DropTable("dbo.Evaluations");
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
            
            CreateTable(
                "dbo.Evaluations",
                c => new
                    {
                        InspectionId = c.Guid(nullable: false),
                        RequirementId = c.Int(nullable: false),
                        Score = c.Double(),
                    })
                .PrimaryKey(t => new { t.InspectionId, t.RequirementId });
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InspectionId = c.Guid(nullable: false),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        FinalScore = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "InspectionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.InspectionEmployees", "Employee_Id");
            CreateIndex("dbo.InspectionEmployees", "Inspection_Id");
            CreateIndex("dbo.Events", "InspectionId");
            CreateIndex("dbo.Evaluations", "RequirementId");
            CreateIndex("dbo.Evaluations", "InspectionId");
            CreateIndex("dbo.Documents", "InspectionId");
            AddForeignKey("dbo.Events", "InspectionId", "dbo.Inspections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Evaluations", "RequirementId", "dbo.Requirements", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Evaluations", "InspectionId", "dbo.Inspections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "InspectionId", "dbo.Inspections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InspectionEmployees", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InspectionEmployees", "Inspection_Id", "dbo.Inspections", "Id", cascadeDelete: true);
        }
    }
}
