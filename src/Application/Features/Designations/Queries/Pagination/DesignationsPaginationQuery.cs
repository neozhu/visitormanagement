// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Designations.DTOs;
using CleanArchitecture.Blazor.Application.Features.Designations.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Designations.Queries.Pagination;

    public class DesignationsWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<DesignationDto>>, ICacheable
    {
        public string CacheKey => DesignationCacheKey.GetPagtionCacheKey("{this}");
        public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(DesignationCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class DesignationsWithPaginationQueryHandler :
         IRequestHandler<DesignationsWithPaginationQuery, PaginatedData<DesignationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DesignationsWithPaginationQueryHandler> _localizer;

        public DesignationsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<DesignationsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<DesignationDto>> Handle(DesignationsWithPaginationQuery request, CancellationToken cancellationToken)
        {
      
           var data = await _context.Designations
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<DesignationDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }