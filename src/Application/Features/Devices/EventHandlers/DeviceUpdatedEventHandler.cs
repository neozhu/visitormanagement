// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Devices.EventHandlers;

    public class DeviceUpdatedEventHandler : INotificationHandler<DomainEventNotification<DeviceUpdatedEvent>>
    {
        private readonly ILogger<DeviceUpdatedEventHandler> _logger;

        public DeviceUpdatedEventHandler(
            ILogger<DeviceUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DeviceUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
