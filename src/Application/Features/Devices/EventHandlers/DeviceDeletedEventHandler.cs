// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Devices.EventHandlers;

    public class DeviceDeletedEventHandler : INotificationHandler<DomainEventNotification<DeviceDeletedEvent>>
    {
        private readonly ILogger<DeviceDeletedEventHandler> _logger;

        public DeviceDeletedEventHandler(
            ILogger<DeviceDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DeviceDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
