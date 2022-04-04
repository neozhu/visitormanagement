// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Delete;

    public class DeleteVisitorCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => VisitorCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource;
      public DeleteVisitorCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteVisitorCommandHandler : 
                 IRequestHandler<DeleteVisitorCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteVisitorCommandHandler> _localizer;
        public DeleteVisitorCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteVisitorCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteVisitorCommand request, CancellationToken cancellationToken)
        {
     
            var items = await _context.Visitors.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
            var deleteevent=new VisitorDeletedEvent(item);
            item.DomainEvents.Add(deleteevent);
                _context.Visitors.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

