// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Related;

public class GetRelatedVisitorQuery : IRequest<List<VisitorDto>?>, ICacheable
{
    public int? EmployeeId { get; private set; }
    public GetRelatedVisitorQuery(int? employeeId)
    {
        EmployeeId = employeeId;
    }


    public string CacheKey => VisitorCacheKey.GetRelatedCacheKey(EmployeeId);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}

public class GetRelatedVisitorQueryHandler :
     IRequestHandler<GetRelatedVisitorQuery, List<VisitorDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetRelatedVisitorQueryHandler> _localizer;

    public GetRelatedVisitorQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetRelatedVisitorQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<List<VisitorDto>?> Handle(GetRelatedVisitorQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Visitors.Where(x => x.EmployeeId == request.EmployeeId && x.Status!=VisitorStatus.Finished)
                              .OrderByDescending(x => x.Id)
                              .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                              .ToListAsync(cancellationToken);
        return data;
    }
}


