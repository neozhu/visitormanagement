// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class CheckinPointConfiguration : IEntityTypeConfiguration<CheckinPoint>
{
    public void Configure(EntityTypeBuilder<CheckinPoint> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.HasMany(a => a.Devices)
               .WithOne(b => b.CheckinPoint)
               .HasForeignKey(b => b.CheckinPointId);
    }
}
