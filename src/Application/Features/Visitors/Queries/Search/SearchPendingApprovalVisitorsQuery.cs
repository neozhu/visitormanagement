// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;
using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Search;

public class SearchPendingApprovalVisitorsQuery : IRequest<List<VisitorDto>>, ICacheable
{
    public string? Keyword { get; private set; }
    public SearchPendingApprovalVisitorsQuery(string? keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.SearchPendingApproval(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}
public class SearchPendingCheckingVisitorsQuery : IRequest<List<VisitorDto>>, ICacheable
{
    public string? Keyword { get; private set; }
    public SearchPendingCheckingVisitorsQuery(string? keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.SearchPendingChecking(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}
public class SearchPendingConfirmVisitorsQuery : IRequest<List<VisitorDto>>, ICacheable
{
    public string? Keyword { get; private set; }
    public SearchPendingConfirmVisitorsQuery(string? keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.SearchPendingConfirm(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}
public class SearchPendingCheckinVisitorsQuery : IRequest<List<VisitorDto>>, ICacheable
{
    public string? Keyword { get; private set; }
    public SearchPendingCheckinVisitorsQuery(string? keyword)
    {
        Keyword = keyword;
    }
    public string CacheKey => VisitorCacheKey.SearchPendingCheckin(Keyword);
    public MemoryCacheEntryOptions? Options => VisitorCacheKey.MemoryCacheEntryOptions;
}
public class SearchPendingApprovalVisitorsQueryHandler :
     IRequestHandler<SearchPendingConfirmVisitorsQuery, List<VisitorDto>>,
     IRequestHandler<SearchPendingApprovalVisitorsQuery, List<VisitorDto>>,
    IRequestHandler<SearchPendingCheckingVisitorsQuery, List<VisitorDto>>,
     IRequestHandler<SearchPendingCheckinVisitorsQuery, List<VisitorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SearchPendingApprovalVisitorsQueryHandler> _localizer;

    public SearchPendingApprovalVisitorsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<SearchPendingApprovalVisitorsQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<List<VisitorDto>> Handle(SearchPendingApprovalVisitorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors.Specify(new SearchPendingApprovalSpecification(request.Keyword)).OrderByDescending(x=>x.Id)
                          .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                          .ToListAsync(cancellationToken);
        if (result is null) return new List<VisitorDto>();
        return result;
    }
    public async Task<List<VisitorDto>> Handle(SearchPendingCheckingVisitorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors.Specify(new SearchPendingCheckingSpecification(request.Keyword)).OrderByDescending(x => x.Id)
                          .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                          .ToListAsync(cancellationToken);
        if (result is null) return new List<VisitorDto>();
        return result;
    }
    public async Task<List<VisitorDto>> Handle(SearchPendingCheckinVisitorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors.Specify(new SearchPendingCheckinSpecification(request.Keyword)).OrderByDescending(x => x.Id)
                          .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                          .ToListAsync(cancellationToken);
        if (result is null) return new List<VisitorDto>();
        return result;
    }

    public async Task<List<VisitorDto>> Handle(SearchPendingConfirmVisitorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.Visitors.Specify(new SearchPendingConfirmSpecification(request.Keyword)).OrderByDescending(x => x.Id)
                           .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
                           .ToListAsync(cancellationToken);
        if (result is null) return new List<VisitorDto>();
        return result;
    }
    public class SearchPendingApprovalSpecification : Specification<Visitor>
    {
        public SearchPendingApprovalSpecification(string? keyword)
        {
            AddInclude(x => x.Employee);
            AddInclude(x => x.Companions);
            AddInclude(x => x.ApprovalHistories);
            Criteria = q => q.Status==VisitorStatus.PendingApproval;
            if (!string.IsNullOrEmpty(keyword))
            {
                And(x => x.Name.Contains(keyword) || x.CompanyName.Contains(keyword) || x.PassCode.Contains(keyword) || x.Email.Contains(keyword) || x.PhoneNumber.Contains(keyword) || x.Employee.Name.Contains(keyword));
            }
             
        }
    }
    public class SearchPendingCheckingSpecification : Specification<Visitor>
    {
        public SearchPendingCheckingSpecification(string? keyword)
        {
            AddInclude(x => x.Employee);
            AddInclude(x => x.Companions);
            AddInclude(x => x.ApprovalHistories);
            Criteria = q => q.Status == VisitorStatus.PendingChecking;
            if (!string.IsNullOrEmpty(keyword))
            {
                And(x => x.Name.Contains(keyword) || x.CompanyName.Contains(keyword) || x.PassCode.Contains(keyword) || x.Email.Contains(keyword) || x.PhoneNumber.Contains(keyword) || x.Employee.Name.Contains(keyword));
            }

        }
    }
    public class SearchPendingCheckinSpecification : Specification<Visitor>
    {
        public SearchPendingCheckinSpecification(string? keyword)
        {
            AddInclude(x => x.Employee);
            AddInclude(x => x.Companions);
            AddInclude(x => x.ApprovalHistories);
            Criteria = q => q.Status==VisitorStatus.PendingCheckin;
            if (!string.IsNullOrEmpty(keyword))
            {
                And(x => x.Name.Contains(keyword) || x.CompanyName.Contains(keyword) || x.PassCode.Contains(keyword) || x.Email.Contains(keyword) || x.PhoneNumber.Contains(keyword) || x.Employee.Name.Contains(keyword));
            }

        }
    }
    public class SearchPendingConfirmSpecification : Specification<Visitor>
    {
        public SearchPendingConfirmSpecification(string? keyword)
        {
            AddInclude(x => x.Employee);
            AddInclude(x => x.Companions);
            AddInclude(x => x.ApprovalHistories);
            Criteria = q => q.Status == VisitorStatus.PendingConfirm || ( q.CheckinDate !=null && q.Status== VisitorStatus.PendingCheckin);
            if (!string.IsNullOrEmpty(keyword))
            {
                And(x => x.Name.Contains(keyword) || x.CompanyName.Contains(keyword) || x.PassCode.Contains(keyword) || x.Email.Contains(keyword) || x.PhoneNumber.Contains(keyword) || x.Employee.Name.Contains(keyword));
            }

        }
    }
}
