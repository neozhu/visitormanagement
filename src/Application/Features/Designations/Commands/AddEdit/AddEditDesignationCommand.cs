// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Designations.DTOs;
using CleanArchitecture.Blazor.Application.Features.Designations.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Designations.Commands.AddEdit;

public class AddEditDesignationCommand : DesignationDto, IRequest<Result<int>>, IMapFrom<Designation>, ICacheInvalidator
{
    public string CacheKey => DesignationCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DesignationCacheKey.SharedExpiryTokenSource;
}

public class AddEditDesignationCommandHandler : IRequestHandler<AddEditDesignationCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditDesignationCommandHandler> _localizer;
    public AddEditDesignationCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditDesignationCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditDesignationCommand request, CancellationToken cancellationToken)
    {

        if (request.Id > 0)
        {
            var item = await _context.Designations.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException("Designation {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            var updateevent = new DesignationUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Designation>(request);
            _context.Designations.Add(item);
            var careateevent = new DesignationCreatedEvent(item);
            item.DomainEvents.Add(careateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

