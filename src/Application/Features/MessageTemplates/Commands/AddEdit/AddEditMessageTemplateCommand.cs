// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.MessageTemplates.DTOs;
using CleanArchitecture.Blazor.Application.Features.MessageTemplates.Caching;
namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.Commands.AddEdit;

public class AddEditMessageTemplateCommand : MessageTemplateDto, IRequest<Result<int>>, IMapFrom<MessageTemplate>, ICacheInvalidator
{
    public string CacheKey => MessageTemplateCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => MessageTemplateCacheKey.SharedExpiryTokenSource();
}

public class AddEditMessageTemplateCommandHandler : IRequestHandler<AddEditMessageTemplateCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditMessageTemplateCommandHandler> _localizer;
    public AddEditMessageTemplateCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditMessageTemplateCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditMessageTemplateCommand request, CancellationToken cancellationToken)
    {

        if (request.Id > 0)
        {
            var item = await _context.MessageTemplates.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException("MessageTemplate {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            // add update domain events if this entity implement the IHasDomainEvent interface
            item.DomainEvents.Add(new UpdatedEvent<MessageTemplate>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<MessageTemplate>(request);
            // add create domain events if this entity implement the IHasDomainEvent interface
            item.DomainEvents.Add(new CreatedEvent<MessageTemplate>(item));
            _context.MessageTemplates.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

