// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Queries.GetAll;

    public class GetAllVisitorHistoriesQuery : IRequest<IEnumerable<VisitorHistoryDto>>, ICacheable
    {
       public string CacheKey => VisitorHistoryCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorHistoryCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class GetAllVisitorHistoriesQueryHandler :
         IRequestHandler<GetAllVisitorHistoriesQuery, IEnumerable<VisitorHistoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllVisitorHistoriesQueryHandler> _localizer;

        public GetAllVisitorHistoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllVisitorHistoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<VisitorHistoryDto>> Handle(GetAllVisitorHistoriesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.VisitorHistories
                         .ProjectTo<VisitorHistoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


