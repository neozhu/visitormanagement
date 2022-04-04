// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Departments.DTOs;
using CleanArchitecture.Blazor.Application.Features.Departments.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Departments.Commands.AddEdit;

public class AddEditDepartmentCommand : DepartmentDto, IRequest<Result<int>>, IMapFrom<Department>, ICacheInvalidator
{
    public string CacheKey => DepartmentCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DepartmentCacheKey.SharedExpiryTokenSource;
}

public class AddEditDepartmentCommandHandler : IRequestHandler<AddEditDepartmentCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditDepartmentCommandHandler> _localizer;
    public AddEditDepartmentCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditDepartmentCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request.Id > 0)
        {
            var item = await _context.Departments.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException("Department {request.Id} Not Found.");
            var updateevent = new DepartmentUpdatedEvent(item);
            item = _mapper.Map(request, item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Department>(request);
            var createevent=new DepartmentCreatedEvent(item);
            item.DomainEvents.Add(createevent);
            _context.Departments.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

