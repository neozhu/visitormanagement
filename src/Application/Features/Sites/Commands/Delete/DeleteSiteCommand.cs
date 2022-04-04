// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Sites.DTOs;
using CleanArchitecture.Blazor.Application.Features.Sites.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Sites.Commands.Delete;

    public class DeleteSiteCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => SiteCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => SiteCacheKey.SharedExpiryTokenSource;
      public DeleteSiteCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteSiteCommandHandler : 
                 IRequestHandler<DeleteSiteCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteSiteCommandHandler> _localizer;
        public DeleteSiteCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteSiteCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Sites.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
            var deleteevent=new SiteDeletedEvent(item);
            item.DomainEvents.Add(deleteevent);
                _context.Sites.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

