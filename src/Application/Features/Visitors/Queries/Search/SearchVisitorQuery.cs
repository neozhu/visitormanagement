// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Search;

public class SearchVisitorQuery : IRequest<VisitorDto?>, ICacheable
{
    public string Keyword { get; private set; }
    public SearchVisitorQuery(string keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.Search(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}
public class SearchVisitorFuzzyQuery : IRequest<List<VisitorDto>>, ICacheable
{
    public string Keyword { get; private set; }
    public SearchVisitorFuzzyQuery(string keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.SearchFuzzy(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}



public class SearchVisitorQueryHandler :
     IRequestHandler<SearchVisitorFuzzyQuery, List<VisitorDto>>,
     IRequestHandler<SearchVisitorQuery, VisitorDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SearchVisitorQueryHandler> _localizer;

    public SearchVisitorQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<SearchVisitorQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<VisitorDto?> Handle(SearchVisitorQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.Visitors.OrderByDescending(x=>x.Id).Include(x=>x.Site).Include(x=>x.Employee).Include(x=>x.Companions).Include(x=>x.ApprovalHistories).FirstOrDefaultAsync(x =>x.PassCode==request.Keyword || x.Email == request.Keyword || x.PhoneNumber == request.Keyword || x.Name == request.Keyword);
        if (item is null) return null;
        var dto= _mapper.Map<VisitorDto>(item);
        return dto;
    }
    public async Task<List<VisitorDto>> Handle(SearchVisitorFuzzyQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors
            .OrderByDescending(x => x.Name)
            .Select(x=>new VisitorDto() {
                Name = x.Name,
                CompanyName=x.CompanyName,
                LicensePlateNumber = x.LicensePlateNumber,
                PhoneNumber=x.PhoneNumber,
                Email=x.Email,
                IdentificationNo =x.IdentificationNo,
                Gender = x.Gender})
            .Distinct()
            .ToListAsync(cancellationToken);
        if (result is null) return new List<VisitorDto>();
        return result;
    }
}


