// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.MessageTemplates.DTOs;
using CleanArchitecture.Blazor.Application.Features.MessageTemplates.Caching;


namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.Commands.Delete;

    public class DeleteMessageTemplateCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => MessageTemplateCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => MessageTemplateCacheKey.SharedExpiryTokenSource();
      public DeleteMessageTemplateCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteMessageTemplateCommandHandler : 
                 IRequestHandler<DeleteMessageTemplateCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteMessageTemplateCommandHandler> _localizer;
        public DeleteMessageTemplateCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteMessageTemplateCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteMessageTemplateCommand request, CancellationToken cancellationToken)
        {
     
            var items = await _context.MessageTemplates.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
			    // add delete domain events if this entity implement the IHasDomainEvent interface
				item.DomainEvents.Add(new DeletedEvent<MessageTemplate>(item));
                _context.MessageTemplates.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

