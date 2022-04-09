// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;


namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Commands.Delete;

    public class DeleteVisitorHistoryCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => VisitorHistoryCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => VisitorHistoryCacheKey.SharedExpiryTokenSource;
      public DeleteVisitorHistoryCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteVisitorHistoryCommandHandler : 
                 IRequestHandler<DeleteVisitorHistoryCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteVisitorHistoryCommandHandler> _localizer;
        public DeleteVisitorHistoryCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteVisitorHistoryCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteVisitorHistoryCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.VisitorHistories.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
                _context.VisitorHistories.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

