using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Common.Models.Enums;
using DataAccess.DbModels;
using DataAccess.Mappers;
using Models;

namespace DataAccess.DataAccess.InspectionRepository
{
    public class InspectionRepository : IInspectionRepository
    {
        public async Task<InspectionModel> CreateInspection(Guid contractorId, Guid customerId)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = new Inspection
                {
                    Id = new Guid(), 
                    ContractorId = contractorId, 
                    CustomerId = customerId, 
                    Status = InspectionStatus.New
                };

                context.Inspections.Add(inspection);

                await context.SaveChangesAsync();

                return inspection.Map();
            }
        }

        public async Task<InspectionModel> StartInspection(Guid inspectionId, List<Guid> assessorIds)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = await context.Inspections.Include(x => x.Contractor.Employees).FirstOrDefaultAsync(x => x.Id == inspectionId);

                if (inspection == null)
                {
                    throw new Exception("Inspection not found.");
                }

                if (inspection.Status != InspectionStatus.New)
                {
                    throw new Exception("Cannot change inspection.");
                }

                inspection.Assessors = inspection.Contractor.Employees
                    .Where(x => x.Role == UserRole.User && assessorIds.Contains(x.Id)).ToList();

                inspection.StartDate = DateTime.Now;
                inspection.Status = InspectionStatus.InProgress;

                await context.SaveChangesAsync();

                return inspection.Map();
            }
        }

        public async Task<InspectionModel> GetInspection(Guid inspectionId)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = await context.Inspections
                    .Include(x => x.Contractor.Employees)
                    .Include(x => x.Schedule)
                    .FirstOrDefaultAsync(x => x.Id == inspectionId);

                if (inspection == null)
                {
                    throw new Exception("Inspection not found.");
                }

                return inspection.Map();
            }
        }

        public async Task<List<EventModel>> AddInspectionEvent(Guid inspectionId, EventModel eventModel)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = await context.Inspections
                    .Include(x => x.Schedule)
                    .FirstOrDefaultAsync(x => x.Id == inspectionId);

                if (inspection == null)
                {
                    throw new Exception("Inspection not found.");
                }

                inspection.Schedule.Add(eventModel.Map());

                await context.SaveChangesAsync();

                return inspection.Schedule.OrderBy(x => x.Date).ToList().Select(x => x.Map()).ToList();
            }
        }

        public async Task<List<EventModel>> DeleteInspectionEvent(Guid inspectionId, Guid eventId)
        {
            using (var context = new ISControlDbContext())
            {
                var e = context.Events.FirstOrDefault(x => x.Id == eventId);

                if (e != null)
                {
                    context.Events.Remove(e);

                    await context.SaveChangesAsync();
                }

                return context.Events.Where(x => x.InspectionId == inspectionId).OrderBy(x => x.Date).ToList().Select(x => x.Map()).ToList();
            }
        }

        public async Task AddInspectionDocument(Guid inspectionId, string documentName, byte[] document)
        {
            using (var context = new ISControlDbContext())
            {
                var inspection = context.Inspections.Include(x => x.Documents)
                    .FirstOrDefault(x => x.Id == inspectionId);

                if (inspection == null)
                {
                    throw new Exception("Inspection not found.");
                }

                if (inspection.Documents.Any(x => x.Name == documentName))
                {
                    throw new Exception("Document with same name already exists");
                }

                context.Documents.Add(new Document
                {
                    Id = Guid.NewGuid(),
                    InspectionId = inspectionId,
                    Name = documentName,
                    Data = document
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<DocumentModel> GetInspectionDocument(Guid documentId)
        {
            using (var context = new ISControlDbContext())
            {
                var document = await context.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

                if (document == null)
                {
                    throw new Exception("Document not found");
                }

                return document.Map();
            }
        }

        public async Task<Page<BriefDocumentModel>> GetDocumentList(Guid inspectionId, int take, int skip)
        {
            using (var context = new ISControlDbContext())
            {
                var documents = context.Documents.Where(x => x.InspectionId == inspectionId);

                return new Page<BriefDocumentModel>
                {
                    Items = documents
                        .OrderBy(x => x.Name)
                        .Skip(skip)
                        .Take(take)
                        .Select(x => new BriefDocumentModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList(),
                    Total = documents.Count()
                };
            }
        }

        public async Task DeleteDocument(Guid documentId)
        {
            using (var context = new ISControlDbContext())
            {
                var document = await context.Documents.FirstOrDefaultAsync(x => x.Id == documentId);

                if (document == null)
                {
                    throw new Exception("Document not found");
                }

                context.Documents.Remove(document);

                await context.SaveChangesAsync();
            }
        }
    }
}