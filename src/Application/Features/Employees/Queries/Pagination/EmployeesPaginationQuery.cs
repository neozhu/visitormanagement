// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Employees.DTOs;
using CleanArchitecture.Blazor.Application.Features.Employees.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Employees.Queries.Pagination;

public class EmployeesWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<EmployeeDto>>, ICacheable
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()},Name:{Name},Email:{Email},DepartmentId:{DepartmentId}";
    }
    public string CacheKey => EmployeeCacheKey.GetPagtionCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(EmployeeCacheKey.SharedExpiryTokenSource.Token));
}

public class EmployeesWithPaginationQueryHandler :
     IRequestHandler<EmployeesWithPaginationQuery, PaginatedData<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<EmployeesWithPaginationQueryHandler> _localizer;

    public EmployeesWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IStringLocalizer<EmployeesWithPaginationQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<PaginatedData<EmployeeDto>> Handle(EmployeesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Employees.Specify(new SearchEmployeeSpecification(request))
              
             .OrderBy($"{request.OrderBy} {request.SortDirection}")
             .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }
}

public class SearchEmployeeSpecification : Specification<Employee>
{
    public SearchEmployeeSpecification(EmployeesWithPaginationQuery query)
    {
        AddInclude(x => x.Department);
        AddInclude(x => x.Designation);
        Criteria = q => q.Name != null;
        if (!string.IsNullOrEmpty(query.Keyword))
        {
            And(x => x.Name.Contains(query.Keyword) || x.About.Contains(query.Keyword) || x.PhoneNumber.Contains(query.Keyword));
        }
        if (!string.IsNullOrEmpty(query.Name))
        {
            And(x => x.Name.Contains(query.Name));
        }
        if (!string.IsNullOrEmpty(query.Email))
        {
            And(x => x.Name.Contains(query.Email));
        }
        if (query.DepartmentId is not null)
        {
            And(x => x.DepartmentId == query.DepartmentId);
        }
        
    }
}