// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.Delete;

public class DeleteDepartmentCommand : IRequest<Result>, ICacheInvalidator
{
    public int[] Id { get; }
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource;
    public DeleteDepartmentCommand(int[] id)
    {
        Id = id;
    }
}

public class DeleteDepartmentCommandHandler :
             IRequestHandler<DeleteDepartmentCommand, Result>

{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DeleteDepartmentCommandHandler> _localizer;
    public DeleteDepartmentCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<DeleteDepartmentCommandHandler> localizer,
         IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {

        var items = await _context.Departments.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            var deleteevent = new DepartmentDeletedEvent(item);
            item.DomainEvents.Add(deleteevent);
            _context.Departments.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

}

