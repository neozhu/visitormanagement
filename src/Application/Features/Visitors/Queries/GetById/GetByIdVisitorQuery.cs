// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.GetById;

public class GetByIdVisitorQuery : IRequest<VisitorDto?>, ICacheable
{
    public int Id { get; private set; }
    public GetByIdVisitorQuery(int id)
    {
        Id = id;
    }


    public string CacheKey => VisitorCacheKey.GetByIdCacheKey(Id);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}

public class GetByIdVisitorQueryQueryHandler :
     IRequestHandler<GetByIdVisitorQuery, VisitorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<GetByIdVisitorQueryQueryHandler> _localizer;

    public GetByIdVisitorQueryQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<GetByIdVisitorQueryQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<VisitorDto?> Handle(GetByIdVisitorQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Visitors.Where(x => x.Id == request.Id)
                        .Include(x=>x.Employee)
                        .Include(x=>x.Companions)
                        .Include(x=>x.ApprovalHistories)
                        .Include(x => x.VisitorHistories).ThenInclude(x=>x.CheckinPoint).ThenInclude(x=>x.Site)
                        .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
        return data.FirstOrDefault();
    }
}


