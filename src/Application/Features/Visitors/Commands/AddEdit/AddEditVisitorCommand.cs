// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.AddEdit;

public class AddEditVisitorCommand : VisitorDto, IRequest<Result<int>>,  ICacheInvalidator
{
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
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
            foreach(var companiondto in request.Companions)
            {
                switch(companiondto.TrackingState)
                {
                    case TrackingState.Added:
                        var companionToAdd = _mapper.Map<Companion>(companiondto);
                        companionToAdd.VisitorId = item.Id;
                        _context.Companions.Add(companionToAdd);
                        break;
                    case TrackingState.Modified:
                        var companionToUpdate = await _context.Companions.FindAsync(new object[] { companiondto.Id }, cancellationToken);
                        if (companionToUpdate is null) continue;
                        companionToUpdate = _mapper.Map(companiondto, companionToUpdate);
                        break;
                    case TrackingState.Deleted:
                        var companionToDelete = await _context.Companions.FindAsync(new object[] { companiondto.Id }, cancellationToken);
                        if (companionToDelete is null) continue;
                        _context.Companions.Remove(companionToDelete);
                        break;
                }
            }
            item.DomainEvents.Add(new UpdatedEvent<Visitor>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Visitor>(request);
            item.Status = VisitorStatus.PendingApproval;
            foreach (var companiondto in request.Companions)
            {
                var companion = _mapper.Map<Companion>(companiondto);
                companion.Visitor = item;
                companion.VisitorId = item.Id;
                item.Companions.Add(companion);
            }
            item.DomainEvents.Add(new CreatedEvent<Visitor>(item));
            _context.Visitors.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

