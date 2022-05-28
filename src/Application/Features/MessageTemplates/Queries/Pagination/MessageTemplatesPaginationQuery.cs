// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.MessageTemplates.DTOs;
using CleanArchitecture.Blazor.Application.Features.MessageTemplates.Caching;

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.Queries.Pagination;

    public class MessageTemplatesWithPaginationQuery : PaginationFilter, IRequest<PaginatedData<MessageTemplateDto>>, ICacheable
    {
        public string CacheKey => MessageTemplateCacheKey.GetPagtionCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => MessageTemplateCacheKey.MemoryCacheEntryOptions;
    }
    
    public class MessageTemplatesWithPaginationQueryHandler :
         IRequestHandler<MessageTemplatesWithPaginationQuery, PaginatedData<MessageTemplateDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<MessageTemplatesWithPaginationQueryHandler> _localizer;

        public MessageTemplatesWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<MessageTemplatesWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<MessageTemplateDto>> Handle(MessageTemplatesWithPaginationQuery request, CancellationToken cancellationToken)
        {
        var data = await _context.MessageTemplates.Where(x =>
                          x.Subject.Contains(request.Keyword) ||
                          x.Body.Contains(request.Keyword) ||
                          x.Description.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<MessageTemplateDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }