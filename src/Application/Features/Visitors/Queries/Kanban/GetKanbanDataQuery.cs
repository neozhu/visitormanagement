// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Kanban;

public class GetKanbanDataQuery : IRequest<List<VisitorStatusSumarryDto>>, ICacheable
{
    public string CacheKey => VisitorCacheKey.GetKanbanCacheKey;
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}

public class GetKanbanDataQueryHandler :
     IRequestHandler<GetKanbanDataQuery, List<VisitorStatusSumarryDto>>
{
    private readonly IApplicationDbContext _context;



    public GetKanbanDataQueryHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
   

    }

    public async Task<List<VisitorStatusSumarryDto>> Handle(GetKanbanDataQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors.Select(x => new VisitorStatusSumarryDto() { Status = x.Status, Id=x.Id, Name = x.Name, CompanyName = x.CompanyName, PhoneNumber = x.PhoneNumber }).ToListAsync();
        return result;

    }
}


