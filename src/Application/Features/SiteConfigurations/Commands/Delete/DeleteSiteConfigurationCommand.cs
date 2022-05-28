// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.DTOs;
using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Caching;


namespace CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Commands.Delete;

    public class DeleteSiteConfigurationCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => SiteConfigurationCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => SiteConfigurationCacheKey.SharedExpiryTokenSource();
      public DeleteSiteConfigurationCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteSiteConfigurationCommandHandler : 
                 IRequestHandler<DeleteSiteConfigurationCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteSiteConfigurationCommandHandler> _localizer;
        public DeleteSiteConfigurationCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteSiteConfigurationCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteSiteConfigurationCommand request, CancellationToken cancellationToken)
        {
          
            var items = await _context.SiteConfigurations.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
			    // add delete domain events if this entity implement the IHasDomainEvent interface
				// item.DomainEvents.Add(new DeletedEvent<SiteConfiguration>(item));
                _context.SiteConfigurations.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

