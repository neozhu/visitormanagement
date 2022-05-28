// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.DTOs;
using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Caching;

namespace CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Queries.GetAll;

    public class GetAllSiteConfigurationsQuery : IRequest<IEnumerable<SiteConfigurationDto>>, ICacheable
    {
       public string CacheKey => SiteConfigurationCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => SiteConfigurationCacheKey.MemoryCacheEntryOptions;
    }
    
    public class GetAllSiteConfigurationsQueryHandler :
         IRequestHandler<GetAllSiteConfigurationsQuery, IEnumerable<SiteConfigurationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllSiteConfigurationsQueryHandler> _localizer;

        public GetAllSiteConfigurationsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllSiteConfigurationsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SiteConfigurationDto>> Handle(GetAllSiteConfigurationsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.SiteConfigurations.OrderBy(x=>x.SiteId)
                         .ProjectTo<SiteConfigurationDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


