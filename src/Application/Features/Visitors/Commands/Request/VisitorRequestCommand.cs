// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Create;

public class VisitorRequestCommand : VisitorDto, IRequest<Result<int>>,  ICacheInvalidator
{
    public VisitorDto? Visitor { get; set; }
    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
}

public class VisitorRequestCommandHandler : IRequestHandler<VisitorRequestCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<VisitorRequestCommandHandler> _localizer;
    public VisitorRequestCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IStringLocalizer<VisitorRequestCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(VisitorRequestCommand request, CancellationToken cancellationToken)
    {
        var userName = await _currentUserService.UserName();
        var item = _mapper.Map<Visitor>(request);
        item.Status = VisitorStatus.PendingVisitor;
        foreach (var companionDto in request.Companions)
        {
            var companion = _mapper.Map<Companion>(companionDto);
            companion.VisitorId = item.Id;
            companion.Visitor = item;
            item.Companions.Add(companion);
        }
        var approval = new ApprovalHistory(){
            Comment = _localizer[VisitorProcess.Request],
            VisitorId = item.Id,
            Visitor = item,
            ProcessingDate = DateTime.Now,
            ApprovedBy = userName
        };
       

        item.DomainEvents.Add(new CreatedEvent<Visitor>(item));
        _context.Visitors.Add(item);
        approval.DomainEvents.Add(new CreatedEvent<ApprovalHistory>(approval));
        _context.ApprovalHistories.Add(approval);
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(item.Id);
    }
}

