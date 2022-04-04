// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.Pagination;

public class DepartmentsWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<DepartmentDto>>, ICacheable
{
    public string CacheKey => DepartmentCacheKey.GetPagtionCacheKey("{this}");
    public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(DepartmentCacheKey.SharedExpiryTokenSource.Token));
}

public class DepartmentsWithPaginationQueryHandler :
     IRequestHandler<DepartmentsWithPaginationQuery, PaginatedData<DepartmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<PaginatedData<DepartmentDto>> Handle(DepartmentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Departments.Where(x => x.Name.Contains(request.Keyword))
             .OrderBy($"{request.OrderBy} {request.SortDirection}")
             .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
             .PaginatedDataAsync(request.PageNumber, request.PageSize);
        return data;
    }
}