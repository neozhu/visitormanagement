// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Create;

public class ApprovalVisitorsCommand : IRequest<Result<int>>, ICacheInvalidator
{
    public int[] VisitorId { get; private set; }
    public string Outcome { get; private set; }
    public ApprovalVisitorsCommand(string outcome, int[] visitorId)
    {
        Outcome = outcome;
        VisitorId = visitorId;
    }
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource;
}

public class ApprovalVisitorsCommandHandler : IRequestHandler<ApprovalVisitorsCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ApprovalVisitorsCommandHandler> _localizer;
    public ApprovalVisitorsCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<ApprovalVisitorsCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(ApprovalVisitorsCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.Visitors.Where(x => request.VisitorId.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            item.ApprovalOutcome = request.Outcome;
            item.Apppoved = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(items.Count);
    }
}

