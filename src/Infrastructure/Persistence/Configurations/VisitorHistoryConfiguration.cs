// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public class VisitorHistoryConfiguration : IEntityTypeConfiguration<VisitorHistory>
{
    public void Configure(EntityTypeBuilder<VisitorHistory> builder)
    {
        builder.Property(e =>e.Attachments).HasConversion(
                v =>(v == null? null: string.Join(',', v)),
                v =>(string.IsNullOrEmpty(v) ? null: v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                );
    }
}
