// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Designations.EventHandlers;

    public class DesignationUpdatedEventHandler : INotificationHandler<DomainEventNotification<DesignationUpdatedEvent>>
    {
        private readonly ILogger<DesignationUpdatedEventHandler> _logger;

        public DesignationUpdatedEventHandler(
            ILogger<DesignationUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DesignationUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
