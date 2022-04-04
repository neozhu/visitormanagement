// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.CheckinPoints.DTOs;
using CleanArchitecture.Blazor.Application.Features.CheckinPoints.Caching;

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.Queries.Pagination;

    public class CheckinPointsWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<CheckinPointDto>>, ICacheable
    {
        public string CacheKey => CheckinPointCacheKey.GetPagtionCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(CheckinPointCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class CheckinPointsWithPaginationQueryHandler :
         IRequestHandler<CheckinPointsWithPaginationQuery, PaginatedData<CheckinPointDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CheckinPointsWithPaginationQueryHandler> _localizer;

        public CheckinPointsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<CheckinPointsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<CheckinPointDto>> Handle(CheckinPointsWithPaginationQuery request, CancellationToken cancellationToken)
        {
           
           var data = await _context.CheckinPoints.Where(x=>x.Name.Contains(request.Keyword)|| x.Description.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<CheckinPointDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }