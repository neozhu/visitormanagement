// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Queries.GetByVisitorId;

public class GetByVisitorIdApprovalHistoriesQuery : IRequest<List<ApprovalHistoryDto>>, ICacheable
{
    public int Id { get; set; }
    public GetByVisitorIdApprovalHistoriesQuery(int id)
    {
        Id = id;
    }

    public string CacheKey => ApprovalHistoryCacheKey.GetByVisitorIdCacheKey(Id);
    public MemoryCacheEntryOptions? Options => ApprovalHistoryCacheKey.MemoryCacheEntryOptions;
}

public class GetByVisitorIdApprovalHistoriesQueryHandler :
     IRequestHandler<GetByVisitorIdApprovalHistoriesQuery, List<ApprovalHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetByVisitorIdApprovalHistoriesQueryHandler> _localizer;

    public GetByVisitorIdApprovalHistoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetByVisitorIdApprovalHistoriesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<List<ApprovalHistoryDto>> Handle(GetByVisitorIdApprovalHistoriesQuery request, CancellationToken cancellationToken)
    {

        var data = await _context.ApprovalHistories.Where(x => x.VisitorId == request.Id)
                         .ProjectTo<ApprovalHistoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
        return data;
    }
}


