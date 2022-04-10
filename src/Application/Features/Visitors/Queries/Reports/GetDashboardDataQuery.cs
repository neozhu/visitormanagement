// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.GetAll;

public class GetDashboardDataQuery : IRequest<Tuple<int, int, int>>, ICacheable
{
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorCacheKey.SharedExpiryTokenSource.Token));
}

public class GetDashboardDataQueryHandler :
     IRequestHandler<GetDashboardDataQuery, Tuple<int, int, int>>
{
    private readonly IApplicationDbContext _context;



    public GetDashboardDataQueryHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
   

    }

    public async Task<Tuple<int, int, int>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        var total = await _context.Visitors.CountAsync(cancellationToken);
        var totalcheckin = await _context.Visitors.CountAsync(x => x.CheckinDate != null && x.CheckoutDate ==null);
        var totalcheckout = await _context.Visitors.CountAsync(x => x.CheckinDate != null && x.CheckoutDate != null);
        return new Tuple<int, int, int>(total, totalcheckin, totalcheckout);
    }
}


