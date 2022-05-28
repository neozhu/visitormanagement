// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Constants;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Commands.Create;

public class CreateVisitorHistoryCommand : VisitorHistoryDto, IRequest<Result<int>>, ICacheInvalidator
{
    public int? CompanionCount { get; set; }
    public int[]? CheckinCompanion { get; set; }
    public string? QrCode { get; set; }
    public int? SiteId { get; set; }
    public string? CurrentStatus { get; set; }
    public List<VisitorHistoryDto> Histories { get; set; } = new();
    public string CacheKey => VisitorHistoryCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorHistoryCacheKey.SharedExpiryTokenSource();
}

public class CreateVisitorHistoryCommandHandler : IRequestHandler<CreateVisitorHistoryCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateVisitorHistoryCommand> _localizer;
    private readonly ILogger<CreateVisitorHistoryCommandHandler> _logger;

    public CreateVisitorHistoryCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<CreateVisitorHistoryCommand> localizer,
        ILogger<CreateVisitorHistoryCommandHandler> logger,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateVisitorHistoryCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<VisitorHistory>(request);
        _context.VisitorHistories.Add(item);
        var visitor = await _context.Visitors.FirstAsync(x => x.Id == request.VisitorId);
        visitor.DomainEvents.Add(new UpdatedEvent<Visitor>(visitor));
        if (item.Stage== CheckStage.Checkin)
        {
            if (visitor.CheckinDate is null)
            {
                visitor.CheckinDate = item.TransitDateTime;
                if(visitor.Status != VisitorStatus.PendingCheckout)
                {
                    if (!request.Companions.Any(x => x.Checked == false && x.CheckinDateTime is null))
                    {
                        visitor.Status = VisitorStatus.PendingConfirm;
                    }
                }

            }
            else
            {
                if (visitor.Status != VisitorStatus.PendingCheckout)
                {
                    if (!request.Companions.Any(x => x.Checked == false && x.CheckinDateTime is null))
                    {
                        visitor.Status = VisitorStatus.PendingConfirm;
                    }
                }
            }
        }
        else if(item.Stage == CheckStage.Checkout)
        {
            if (visitor.CheckoutDate is null)
            {
                visitor.CheckoutDate = item.TransitDateTime;
                if (!request.Companions.Any(x => x.Checked == false && x.CheckoutDateTime is null))
                {
                    visitor.Status = VisitorStatus.PendingFeedback;
                }
            }
        }

        var companionId = request.Companions.Where(x => x.Checked).Select(x => x.Id).ToArray();
        if (companionId is not null && companionId.Any())
        {
            var companions = _context.Companions.Where(x => companionId.Contains(x.Id)).ToList();
            foreach (var comp in companions)
            {
                if (item.Stage == CheckStage.Checkin)
                {
                    if (comp.CheckinDateTime is null)
                    {
                        comp.CheckinDateTime = item.TransitDateTime;
                    }
                }
                else
                {
                    if (comp.CheckoutDateTime is null)
                    {
                        comp.CheckoutDateTime = item.TransitDateTime;
                    }
                }
                _context.Companions.Update(comp);
            }
        }
        VisitorCacheKey.SharedExpiryTokenSource().Cancel();
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{handler}: {Stage}: {TransitDateTime}",nameof(CreateVisitorHistoryCommandHandler), item.Stage, item.TransitDateTime);
        return Result<int>.Success(item.Id);
    }
}

