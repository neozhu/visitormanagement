// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.ApprovalHistories.Queries.GetAll;

    public class GetAllApprovalHistoriesQuery : IRequest<IEnumerable<ApprovalHistoryDto>>, ICacheable
    {
       public string CacheKey => ApprovalHistoryCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => ApprovalHistoryCacheKey.MemoryCacheEntryOptions;
    }
    
    public class GetAllApprovalHistoriesQueryHandler :
         IRequestHandler<GetAllApprovalHistoriesQuery, IEnumerable<ApprovalHistoryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllApprovalHistoriesQueryHandler> _localizer;

        public GetAllApprovalHistoriesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllApprovalHistoriesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ApprovalHistoryDto>> Handle(GetAllApprovalHistoriesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.ApprovalHistories
                         .ProjectTo<ApprovalHistoryDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


