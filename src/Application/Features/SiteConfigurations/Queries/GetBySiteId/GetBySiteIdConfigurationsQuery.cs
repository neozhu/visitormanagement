// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.DTOs;
using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Caching;

namespace CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Queries.GetAll;

public class GetBySiteIdConfigurationsQuery : IRequest<SiteConfigurationDto?>, ICacheable
{
    public int? SiteId { get; private set; }
    public GetBySiteIdConfigurationsQuery(int? siteId)
    {
        SiteId = siteId;
    }
    public string CacheKey => SiteConfigurationCacheKey.GetBySiteIdCacheKey(SiteId);
    public MemoryCacheEntryOptions? Options => SiteConfigurationCacheKey.MemoryCacheEntryOptions;
}

public class GetBySiteIdConfigurationsQueryHandler :
     IRequestHandler<GetBySiteIdConfigurationsQuery, SiteConfigurationDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetBySiteIdConfigurationsQueryHandler> _localizer;

    public GetBySiteIdConfigurationsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetBySiteIdConfigurationsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<SiteConfigurationDto?> Handle(GetBySiteIdConfigurationsQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.SiteConfigurations.Where(x => x.SiteId == request.SiteId)
                     .ProjectTo<SiteConfigurationDto>(_mapper.ConfigurationProvider)
                     .FirstOrDefaultAsync(cancellationToken);
        return data;
    }
}


