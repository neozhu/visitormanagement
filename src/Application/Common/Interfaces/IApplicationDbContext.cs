// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Logger> Loggers { get; set; }
    DbSet<AuditTrail> AuditTrails { get; set; }
    DbSet<DocumentType> DocumentTypes { get; set; }
    DbSet<Document> Documents { get; set; }
    DbSet<KeyValue> KeyValues { get; set; }
    DbSet<Product> Products { get; set; }

    DbSet<Department> Departments { get; set; }
    DbSet<Designation> Designations { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Visitor> Visitors { get; set; }
    DbSet<VisitorHistory> VisitorHistories { get; set; }
    DbSet<Site> Sites { get; set; }
    DbSet<CheckinPoint> CheckinPoints { get; set; }
    DbSet<Device> Devices { get; set; }

    ChangeTracker ChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
