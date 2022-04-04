// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;
using CleanArchitecture.Blazor.Application.Features.CheckinPoints.Caching;

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.Queries.GetAll;

    public class GetAllCheckinPointsQuery : IRequest<IEnumerable<CheckinPointDto>>, ICacheable
    {
       public string CacheKey => CheckinPointCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(CheckinPointCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class GetAllCheckinPointsQueryHandler :
         IRequestHandler<GetAllCheckinPointsQuery, IEnumerable<CheckinPointDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllCheckinPointsQueryHandler> _localizer;

        public GetAllCheckinPointsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllCheckinPointsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<CheckinPointDto>> Handle(GetAllCheckinPointsQuery request, CancellationToken cancellationToken)
        {
    
            var data = await _context.CheckinPoints.OrderBy(x=>x.Name)
                         .ProjectTo<CheckinPointDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


