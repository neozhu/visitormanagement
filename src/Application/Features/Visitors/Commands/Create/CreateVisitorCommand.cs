// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Create;

public class CreateVisitorCommand : VisitorDto, IRequest<Result<int>>, IMapFrom<Visitor>, ICacheInvalidator
{
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource;
}

public class CreateVisitorCommandHandler : IRequestHandler<CreateVisitorCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateVisitorCommand> _localizer;
    public CreateVisitorCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<CreateVisitorCommand> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateVisitorCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Visitor>(request);
        var createevent = new VisitorCreatedEvent(item);
        item.DomainEvents.Add(createevent);
        _context.Visitors.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(item.Id);
    }
}

