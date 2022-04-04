// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Employees.DTOs;
using CleanArchitecture.Blazor.Application.Features.Employees.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Employees.Commands.AddEdit;

public class AddEditEmployeeCommand : EmployeeDto, IRequest<Result<int>>, IMapFrom<Employee>, ICacheInvalidator
{
    public string CacheKey => EmployeeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => EmployeeCacheKey.SharedExpiryTokenSource;
}

public class AddEditEmployeeCommandHandler : IRequestHandler<AddEditEmployeeCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditEmployeeCommandHandler> _localizer;
    public AddEditEmployeeCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditEmployeeCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditEmployeeCommand request, CancellationToken cancellationToken)
    {

        if (request.Id > 0)
        {
            var item = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException("Employee {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            var updateevent = new EmployeeUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Employee>(request);
            _context.Employees.Add(item);
            var createevent=new EmployeeCreatedEvent(item);
            item.DomainEvents.Add(createevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

