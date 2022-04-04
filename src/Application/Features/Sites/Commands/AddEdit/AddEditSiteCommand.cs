// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Sites.DTOs;
using CleanArchitecture.Blazor.Application.Features.Sites.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Sites.Commands.AddEdit;

public class AddEditSiteCommand : SiteDto, IRequest<Result<int>>, IMapFrom<Site>, ICacheInvalidator
{
    public string CacheKey => SiteCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => SiteCacheKey.SharedExpiryTokenSource;
}

public class AddEditSiteCommandHandler : IRequestHandler<AddEditSiteCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditSiteCommandHandler> _localizer;
    public AddEditSiteCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditSiteCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditSiteCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Sites.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException($"Site {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            var updateevent = new SiteUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Site>(request);
            var createevent=new SiteCreatedEvent(item);
            item.DomainEvents.Add(createevent);
            _context.Sites.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

