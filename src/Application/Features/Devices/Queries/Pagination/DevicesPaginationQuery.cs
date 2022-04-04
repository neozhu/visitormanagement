// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Devices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Devices.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Devices.Queries.Pagination;

    public class DevicesWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<DeviceDto>>, ICacheable
    {
        public string CacheKey => DeviceCacheKey.GetPagtionCacheKey("{this}");
        public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(DeviceCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class DevicesWithPaginationQueryHandler :
         IRequestHandler<DevicesWithPaginationQuery, PaginatedData<DeviceDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DevicesWithPaginationQueryHandler> _localizer;

        public DevicesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<DevicesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<DeviceDto>> Handle(DevicesWithPaginationQuery request, CancellationToken cancellationToken)
        {
     
           var data = await _context.Devices.Where(x=>x.Name.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }