// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Commands.Update;

public class UpdateVisitorSurveyResponseCommand : IRequest<Result>, ICacheInvalidator
{
    public int Id { get; set; }
    public int? ResponseValue { get; set; }
    public UpdateVisitorSurveyResponseCommand(int id, int? responseValue)
    {
        Id = id;
        ResponseValue = responseValue;
    }

    public string CacheKey => VisitorCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorCacheKey.SharedExpiryTokenSource();
}

public class UpdateVisitorSurveyResponseCommandHandler : IRequestHandler<UpdateVisitorSurveyResponseCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<UpdateVisitorCommandHandler> _localizer;
    public UpdateVisitorSurveyResponseCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,        
        IStringLocalizer<UpdateVisitorCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _currentUserService = currentUserService;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(UpdateVisitorSurveyResponseCommand request, CancellationToken cancellationToken)
    {
        var userName =await _currentUserService.UserName();
        var item = await _context.Visitors.FindAsync(new object[] { request.Id }, cancellationToken);
        if (item != null)
        {
            var approval = new ApprovalHistory()
            {
                Comment = "Customer Survey Response",
                Outcome = $"Response value: {request.ResponseValue}",
                VisitorId = item.Id,
                ProcessingDate = DateTime.Now,
                ApprovedBy = userName
            };
            approval.DomainEvents.Add(new CreatedEvent<ApprovalHistory>(approval));
            _context.ApprovalHistories.Add(approval);
            
            item.Status = VisitorStatus.Finished;
            item.SurveyResponseValue = request.ResponseValue;
            var updateevent = new UpdatedEvent<Visitor>(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return Result.Success();
    }
}

