// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.MessageTemplates.DTOs;
using CleanArchitecture.Blazor.Application.Features.MessageTemplates.Caching;

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.Queries.GetAll;

    public class GetAllMessageTemplatesQuery : IRequest<IEnumerable<MessageTemplateDto>>, ICacheable
    {
       public string CacheKey => MessageTemplateCacheKey.GetAllCacheKey;
    public MemoryCacheEntryOptions? Options => MessageTemplateCacheKey.MemoryCacheEntryOptions;
    }
    
    public class GetAllMessageTemplatesQueryHandler :
         IRequestHandler<GetAllMessageTemplatesQuery, IEnumerable<MessageTemplateDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllMessageTemplatesQueryHandler> _localizer;

        public GetAllMessageTemplatesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllMessageTemplatesQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IEnumerable<MessageTemplateDto>> Handle(GetAllMessageTemplatesQuery request, CancellationToken cancellationToken)
        {
        
            var data = await _context.MessageTemplates.OrderBy(x=>x.SiteId)
                         .ProjectTo<MessageTemplateDto>(_mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);
            return data;
        }
    }


