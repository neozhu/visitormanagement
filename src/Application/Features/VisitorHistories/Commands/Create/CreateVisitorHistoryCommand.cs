// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Commands.Create;

    public class CreateVisitorHistoryCommand: VisitorHistoryDto,IRequest<Result<int>>, IMapFrom<VisitorHistory>, ICacheInvalidator
    {
      public string CacheKey => VisitorHistoryCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => VisitorHistoryCacheKey.SharedExpiryTokenSource;
    }
    
    public class CreateVisitorHistoryCommandHandler : IRequestHandler<CreateVisitorHistoryCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateVisitorHistoryCommand> _localizer;
        public CreateVisitorHistoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateVisitorHistoryCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(CreateVisitorHistoryCommand request, CancellationToken cancellationToken)
        {
           var item = _mapper.Map<VisitorHistory>(request);
           _context.VisitorHistories.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  Result<int>.Success(item.Id);
        }
    }

