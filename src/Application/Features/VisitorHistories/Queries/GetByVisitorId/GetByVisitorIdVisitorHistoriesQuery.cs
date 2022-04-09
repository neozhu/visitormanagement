// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Queries.GetAll;

public class GetByVisitorIdVisitorHistoriesQuery : IRequest<IEnumerable<VisitorHistoryDto>>, ICacheable
{
    public int? VisitorId { get; private set; }
    public string CacheKey => VisitorHistoryCacheKey.GetByVisitorIdCacheKey(VisitorId);
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorHistoryCacheKey.SharedExpiryTokenSource.Token));
    public GetByVisitorIdVisitorHistoriesQuery(int? visitorId)
    {
        VisitorId = visitorId;
    }
}

public class GetByVisitorIdVisitorHistoriesQueryHandler :
     IRequestHandler<GetByVisitorIdVisitorHistoriesQuery, IEnumerable<VisitorHistoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetAllVisitorHistoriesQueryHandler> _localizer;

    public GetByVisitorIdVisitorHistoriesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetAllVisitorHistoriesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<IEnumerable<VisitorHistoryDto>> Handle(GetByVisitorIdVisitorHistoriesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.VisitorHistories.Where(x => x.VisitorId == request.VisitorId)
                         .ProjectTo<VisitorHistoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
        return data;
    }
}


