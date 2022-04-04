// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Departments.Queries.GetAll;

    public class GetAllDepartmentsQuery : IRequest<IEnumerable<DepartmentDto>>, ICacheable
    {
       public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
       public MemoryCacheEntryOptions? Options => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(DepartmentCacheKey.SharedExpiryTokenSource.Token));
    }
    
    public class GetAllDepartmentsQueryHandler :
         IRequestHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllDepartmentsQueryHandler> _localizer;

        public GetAllDepartmentsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllDepartmentsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Departments.OrderBy(x => x.Name)
                         .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


