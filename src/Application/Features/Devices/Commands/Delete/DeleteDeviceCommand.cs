// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Devices.DTOs;
using CleanArchitecture.Blazor.Application.Features.Devices.Caching;


namespace CleanArchitecture.Blazor.Application.Features.Devices.Commands.Delete;

    public class DeleteDeviceCommand: IRequest<Result>, ICacheInvalidator
    {
      public int[] Id {  get; }
      public string CacheKey => DeviceCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => DeviceCacheKey.SharedExpiryTokenSource;
      public DeleteDeviceCommand(int[] id)
      {
        Id = id;
      }
    }

    public class DeleteDeviceCommandHandler : 
                 IRequestHandler<DeleteDeviceCommand, Result>

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteDeviceCommandHandler> _localizer;
        public DeleteDeviceCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteDeviceCommandHandler> localizer,
             IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
        
            var items = await _context.Devices.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
            var deleteevent = new DeviceDeletedEvent(item);
            item.DomainEvents.Add(deleteevent); 
                _context.Devices.Remove(item);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }

