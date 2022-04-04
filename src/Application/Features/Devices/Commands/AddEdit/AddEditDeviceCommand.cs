// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Devices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Devices.Caching;
namespace CleanArchitecture.Blazor.Application.Features.Devices.Commands.AddEdit;

public class AddEditDeviceCommand : DeviceDto, IRequest<Result<int>>, IMapFrom<Device>, ICacheInvalidator
{
    public string CacheKey => DeviceCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => DeviceCacheKey.SharedExpiryTokenSource;
}

public class AddEditDeviceCommandHandler : IRequestHandler<AddEditDeviceCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditDeviceCommandHandler> _localizer;
    public AddEditDeviceCommandHandler(
        IApplicationDbContext context,
        IStringLocalizer<AddEditDeviceCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _mapper = mapper;
    }
    public async Task<Result<int>> Handle(AddEditDeviceCommand request, CancellationToken cancellationToken)
    {

        if (request.Id > 0)
        {
            var item = await _context.Devices.FindAsync(new object[] { request.Id }, cancellationToken);
            _ = item ?? throw new NotFoundException($"Device {request.Id} Not Found.");
            item = _mapper.Map(request, item);
            var updateevent = new DeviceUpdatedEvent(item);
            item.DomainEvents.Add(updateevent);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }
        else
        {
            var item = _mapper.Map<Device>(request);
            var careateevent = new DeviceCreatedEvent(item);
            item.DomainEvents.Add(careateevent);
            _context.Devices.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<int>.Success(item.Id);
        }

    }
}

