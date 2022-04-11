// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Search;

public class SearchVisitorQuery : IRequest<VisitorDto?>, ICacheable
{
    public string Keyword { get; private set; }
    public SearchVisitorQuery(string keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.Search(Keyword);
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorCacheKey.SharedExpiryTokenSource.Token));
}

public class SearchVisitorQueryHandler :
     IRequestHandler<SearchVisitorQuery, VisitorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SearchVisitorQueryHandler> _localizer;

    public SearchVisitorQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<SearchVisitorQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<VisitorDto?> Handle(SearchVisitorQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.Visitors.OrderByDescending(x=>x.Id).Include(x=>x.Employee).FirstOrDefaultAsync(x =>x.PassCode==request.Keyword || x.Email == request.Keyword || x.PhoneNumber == request.Keyword || x.Name == request.Keyword);
        if (item is null) return null;
        var dto= _mapper.Map<VisitorDto>(item);
        return dto;
    }
}


