// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Designations.DTOs;
using CleanArchitecture.Blazor.Application.Features.Designations.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Designations.Commands.Delete;

public class DeleteDesignationCommand : IRequest<Result>, ICacheInvalidator
{
    public int[] Id { get; }
    public string CacheKey => DesignationCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DesignationCacheKey.SharedExpiryTokenSource;
    public DeleteDesignationCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteDesignationCommandHandler :
             IRequestHandler<DeleteDesignationCommand, Result>

{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteDesignationCommandHandler> _localizer;
    public DeleteDesignationCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteDesignationCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
    {

        var items = await _context.Designations.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            var deleteevent = new DesignationDeletedEvent(item);
            item.DomainEvents.Add(deleteevent);
            _context.Designations.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}

