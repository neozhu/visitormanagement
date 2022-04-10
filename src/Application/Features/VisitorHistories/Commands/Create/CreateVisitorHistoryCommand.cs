// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.VisitorHistories.Commands.Create;

public class CreateVisitorHistoryCommand : VisitorHistoryDto, IRequest<Result<int>>, IMapFrom<VisitorHistory>, ICacheInvalidator
{
    public string CacheKey => VisitorHistoryCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => VisitorHistoryCacheKey.SharedExpiryTokenSource;
}

public class CreateVisitorHistoryCommandHandler : IRequestHandler<CreateVisitorHistoryCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<CreateVisitorHistoryCommand> _localizer;
    public CreateVisitorHistoryCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<CreateVisitorHistoryCommand> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(CreateVisitorHistoryCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<VisitorHistory>(request);
        _context.VisitorHistories.Add(item);
        var visitor = await _context.Visitors.FirstAsync(x => x.Id == request.VisitorId);
        if (visitor.CheckinDate is null)
        {
            visitor.CheckinDate = item.TransitDateTime;
            
        }
        if (item.CheckinPointId >= 2 && visitor.CheckoutDate is null)
        {
            visitor.CheckoutDate = item.TransitDateTime;
        }
        
        VisitorCacheKey.SharedExpiryTokenSource.Cancel();
        await _context.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(item.Id);
    }
}

