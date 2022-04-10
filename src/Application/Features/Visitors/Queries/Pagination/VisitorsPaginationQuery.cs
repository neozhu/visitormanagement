// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
using CleanArchitecture.Blazor.Application.Features.Visitors.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Queries.Pagination;

public class VisitorsWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<VisitorDto>>, ICacheable
{
    public string? PassCode { get; set; }
    public string? Name { get; set; }
    public string? LicensePlateNumber { get; set; }
    public string? CompanyName { get; set; }
    public string? Purpose { get; set; }
    public string? Employee { get; set; }
    public DateTime? ExpectedDate1 { get; set; }
    public DateTime? ExpectedDate2 { get; set; }
    public string? Outcome { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()},Name:{Name},LicensePlateNumber:{LicensePlateNumber},CompanyName:{CompanyName},Purpose:{Purpose},Employee:{Employee},ExpectedDate1:{ExpectedDate1?.ToString()},ExpectedDate2:{ExpectedDate2?.ToString()},Outcome:{Outcome}";
    }
    public string CacheKey => VisitorCacheKey.GetPagtionCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(VisitorCacheKey.SharedExpiryTokenSource.Token));
}

public class VisitorsWithPaginationQueryHandler :
     IRequestHandler<VisitorsWithPaginationQuery, PaginatedData<VisitorDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<VisitorsWithPaginationQueryHandler> _localizer;

    public VisitorsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<VisitorsWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<VisitorDto>> Handle(VisitorsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Visitors.Specify(new SearchVisitorSpecification(request))
          .OrderBy($"{request.OrderBy} {request.SortDirection}")
          .ProjectTo<VisitorDto>(_mapper.ConfigurationProvider)
          .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }

}

public class SearchVisitorSpecification : Specification<Visitor>
{
    public SearchVisitorSpecification(VisitorsWithPaginationQuery query)
    {
        AddInclude(x => x.Employee);
        AddInclude(x => x.Designation);
        Criteria = q => q.Name != null;
        if (!string.IsNullOrEmpty(query.PassCode))
        {
            And(x => x.PassCode.Contains(query.PassCode));
        }
        if (!string.IsNullOrEmpty(query.Keyword))
        {
            And(x => x.Name.Contains(query.Keyword) || x.Comment.Contains(query.Keyword) || x.CompanyName.Contains(query.Keyword));
        }
        if (!string.IsNullOrEmpty(query.Name))
        {
            And(x => x.Name.Contains(query.Name));
        }
        if (!string.IsNullOrEmpty(query.LicensePlateNumber))
        {
            And(x => x.LicensePlateNumber.Contains(query.LicensePlateNumber));
        }
        if (!string.IsNullOrEmpty(query.CompanyName))
        {
            And(x => x.CompanyName.Contains(query.CompanyName));
        }
        if (!string.IsNullOrEmpty(query.Employee))
        {
            And(x => x.Employee.Name.Contains(query.Employee));
        }
        if (!string.IsNullOrEmpty(query.Purpose))
        {
            And(x => x.Purpose==query.Purpose);
        }
        if(query.Outcome is not null)
        {
            And(x => x.ApprovalOutcome.Contains(query.Outcome));
        }
        if (query.ExpectedDate1 is not null && query.ExpectedDate2 is not null)
        {
            And(x => x.ExpectedDate >= query.ExpectedDate1 && x.ExpectedDate < query.ExpectedDate2.Value.AddDays(1));
        }
    }
}