// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Queries.Pagination;

public class ApprovalHistoriesWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<ApprovalHistoryDto>>, ICacheable
{
    public string CacheKey => ApprovalHistoryCacheKey.GetPagtionCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => ApprovalHistoryCacheKey.MemoryCacheEntryOptions;
}

public class ApprovalHistoriesWithPaginationQueryHandler :
     IRequestHandler<ApprovalHistoriesWithPaginationQuery, PaginatedData<ApprovalHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ApprovalHistoriesWithPaginationQueryHandler> _localizer;

    public ApprovalHistoriesWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<ApprovalHistoriesWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<ApprovalHistoryDto>> Handle(ApprovalHistoriesWithPaginationQuery request, CancellationToken cancellationToken)
    {

        var data = await _context.ApprovalHistories.Where(x => x.Visitor.Name.Contains(request.Keyword) || x.ApprovedBy.Contains(request.Keyword))
             .Include(x=>x.Visitor)
             .OrderBy($"{request.OrderBy} {request.SortDirection}")
             .ProjectTo<ApprovalHistoryDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }
}