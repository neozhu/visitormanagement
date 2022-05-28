// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Checking;

public class CheckingVisitorsCommand : IRequest<Result<int>>, ICacheInvalidator
{
    public int[] VisitorId { get; private set; }
    public string Outcome { get; private set; }
    public string? Comment { get; private set; }
    public CheckingVisitorsCommand(string outcome, int[] visitorId,string? comment=null)
    {
        Outcome = outcome;
        VisitorId = visitorId;
        Comment = comment;
    }
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
}

public class CheckingVisitorsCommandHandler : IRequestHandler<CheckingVisitorsCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CheckingVisitorsCommandHandler> _localizer;
    public CheckingVisitorsCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStringLocalizer<CheckingVisitorsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CheckingVisitorsCommand request, CancellationToken cancellationToken)
    {
        var userName = await _currentUserService.UserName();
        var items = await _context.Visitors.Where(x => request.VisitorId.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.ApprovalOutcome = request.Outcome;
            item.ApprovalComment = request.Comment;
            item.Apppoved = true;
            if (item.ApprovalOutcome == ApprovalOutcome.Approved)
            {
                item.Status = VisitorStatus.PendingCheckin;
            }
            else
            {
                item.Status = VisitorStatus.Canceled ;
            }
            var approval = new ApprovalHistory()
            {
                Comment = request.Comment,
                Outcome = request.Outcome,
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

