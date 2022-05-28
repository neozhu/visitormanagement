// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.DTOs;
using CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Caching;
namespace CleanArchitecture.Blazor.Application.Features.SiteConfigurations.Commands.AddEdit;

    public class AddEditSiteConfigurationCommand: SiteConfigurationDto,IRequest<Result<int>>, IMapFrom<SiteConfiguration>, ICacheInvalidator
    {
      public string CacheKey => SiteConfigurationCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => SiteConfigurationCacheKey.SharedExpiryTokenSource();
    }

    public class AddEditSiteConfigurationCommandHandler : IRequestHandler<AddEditSiteConfigurationCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditSiteConfigurationCommandHandler> _localizer;
        public AddEditSiteConfigurationCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<AddEditSiteConfigurationCommandHandler> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result<int>> Handle(AddEditSiteConfigurationCommand request, CancellationToken cancellationToken)
        {
           
            if (request.Id > 0)
            {
                var item = await _context.SiteConfigurations.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException("SiteConfiguration {request.Id} Not Found.");
                item = _mapper.Map(request, item);
				// add update domain events if this entity implement the IHasDomainEvent interface
				// item.DomainEvents.Add(new UpdatedEvent<SiteConfiguration>(item));
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
            else
            {
                var item = _mapper.Map<SiteConfiguration>(request);
                // add create domain events if this entity implement the IHasDomainEvent interface
				// item.DomainEvents.Add(new CreatedEvent<SiteConfiguration>(item));
                _context.SiteConfigurations.Add(item);
                await _context.SaveChangesAsync(cancellationToken);
                return Result<int>.Success(item.Id);
            }
           
        }
    }

