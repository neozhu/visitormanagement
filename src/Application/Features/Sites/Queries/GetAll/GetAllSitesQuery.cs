// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Sites.DTOs;
using CleanArchitecture.Blazor.Application.Features.Sites.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Sites.Queries.GetAll;

    public class GetAllSitesQuery : IRequest<IEnumerable<SiteDto>>, ICacheable
    {
       public string CacheKey => SiteCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(SiteCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class GetAllSitesQueryHandler :
         IRequestHandler<GetAllSitesQuery, IEnumerable<SiteDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllSitesQueryHandler> _localizer;

        public GetAllSitesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllSitesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SiteDto>> Handle(GetAllSitesQuery request, CancellationToken cancellationToken)
        {

            var data = await _context.Sites.OrderBy(x=>x.Name)
                         .ProjectTo<SiteDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


