// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.AddEdit;

public class AddEditVisitorCommand : VisitorDto, IRequest<Result<int>>, IMapFrom<Visitor>, ICacheInvalidator
{
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource;
}

public class AddEditVisitorCommandHandler : IRequestHandler<AddEditVisitorCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditVisitorCommandHandler> _localizer;
    public AddEditVisitorCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditVisitorCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditVisitorCommand request, CancellationToken cancellationToken)
    {

        if (request.Id > 0)
        {
            var item = await _context.Visitors.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException("Visitor {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            var updateevent = new VisitorUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Visitor>(request);
            var createevent=new VisitorCreatedEvent(item);
            item.DomainEvents.Add(createevent);
            _context.Visitors.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

