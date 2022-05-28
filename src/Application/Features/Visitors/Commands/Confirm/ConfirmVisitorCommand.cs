// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Approve;

public class ConfirmVisitorCommand : IRequest<Result<int>>, ICacheInvalidator
{
    public int[] VisitorId { get; private set; }

    public ConfirmVisitorCommand(int[] visitorId)
    {
        VisitorId = visitorId;
    }
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
}

public class ConfirmVisitorCommandCommandHandler : IRequestHandler<ConfirmVisitorCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ConfirmVisitorCommandCommandHandler> _localizer;
    public ConfirmVisitorCommandCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStringLocalizer<ConfirmVisitorCommandCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(ConfirmVisitorCommand request, CancellationToken cancellationToken)
    {
        var userName = await _currentUserService.UserName();
        var items = await _context.Visitors.Where(x => request.VisitorId.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.Status = VisitorStatus.PendingCheckout;
            var approval = new ApprovalHistory()
            {
                Comment = _localizer[VisitorProcess.Confirm],
                VisitorId = item.Id,
                ProcessingDate = DateTime.Now,
                ApprovedBy= userName
            };
            approval.DomainEvents.Add(new CreatedEvent<ApprovalHistory>(approval));
            _context.ApprovalHistories.Add(approval);
            item.DomainEvents.Add(new UpdatedEvent<Visitor>(item));
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(items.Count);
    }
}

