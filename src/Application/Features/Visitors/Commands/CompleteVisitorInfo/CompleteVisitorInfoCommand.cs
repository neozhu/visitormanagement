// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.CompleteVisitorInfo;

public class CompleteVisitorInfoCommand : VisitorDto, IRequest<Result>, IMapFrom<Visitor>, ICacheInvalidator
{
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
}

public class CompleteVisitorInfoCommandHandler : IRequestHandler<CompleteVisitorInfoCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CompleteVisitorInfoCommandHandler> _localizer;
    public CompleteVisitorInfoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStringLocalizer<CompleteVisitorInfoCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(CompleteVisitorInfoCommand request, CancellationToken cancellationToken)
    {
        var userName = await _currentUserService.UserName();
        var item = await _context.Visitors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            item = _mapper.Map(request, item);
            item.Status = VisitorStatus.PendingApproval;
            foreach (var companiondto in request.Companions)
            {
                switch (companiondto.TrackingState)
                {
                    case TrackingState.Added:
                        var companionToAdd = _mapper.Map<Companion>(companiondto);
                        companionToAdd.VisitorId = item.Id;
                        _context.Companions.Add(companionToAdd);
                        break;
                    case TrackingState.Modified:
                        var companionToUpdate = await _context.Companions.FindAsync(new object[] { companiondto.Id }, cancellationToken);
                        if (companionToUpdate is null) continue;
                        _ = _mapper.Map(companiondto, companionToUpdate);
                        break;
                    case TrackingState.Deleted:
                        var companionToDelete = await _context.Companions.FindAsync(new object[] { companiondto.Id }, cancellationToken);
                        if (companionToDelete is null) continue;
                        _context.Companions.Remove(companionToDelete);
                        break;
                }
            }
            var approval = new ApprovalHistory()
            {
                Comment = _localizer[VisitorProcess.CompleteInfo],
                VisitorId = item.Id,
                ProcessingDate = DateTime.Now,
                ApprovedBy = userName,
            };
            approval.DomainEvents.Add(new CreatedEvent<ApprovalHistory>(approval));
            _context.ApprovalHistories.Add(approval);
            item.DomainEvents.Add(new UpdatedEvent<Visitor>(item));
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}

