// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Employees.DTOs;
using CleanArchitecture.Blazor.Application.Features.Employees.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Employees.Queries.GetAll;

    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>, ICacheable
    {
       public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(EmployeeCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class GetAllEmployeesQueryHandler :
         IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllEmployeesQueryHandler> _localizer;

        public GetAllEmployeesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllEmployeesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Employees.OrderBy(x=>x.Name)
                         .ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


