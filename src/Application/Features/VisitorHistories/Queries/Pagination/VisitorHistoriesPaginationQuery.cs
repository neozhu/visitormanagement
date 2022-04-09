// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Queries.Pagination;

    public class VisitorHistoriesWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<VisitorHistoryDto>>, ICacheable
    {
        public string CacheKey => VisitorHistoryCacheKey.GetPagtionCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorHistoryCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class VisitorHistoriesWithPaginationQueryHandler :
         IRequestHandler<VisitorHistoriesWithPaginationQuery, PaginatedData<VisitorHistoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<VisitorHistoriesWithPaginationQueryHandler> _localizer;

        public VisitorHistoriesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<VisitorHistoriesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<VisitorHistoryDto>> Handle(VisitorHistoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
           var data = await _context.VisitorHistories.Where(x=>x.Visitor.Name.Contains(request.Keyword) || x.Visitor.CompanyName.Contains(request.Keyword) || x.Comment.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<VisitorHistoryDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }