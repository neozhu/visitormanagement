// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Designations.EventHandlers;

    public class DesignationCreatedEventHandler : INotificationHandler<DomainEventNotification<DesignationCreatedEvent>>
    {
        private readonly ILogger<DesignationCreatedEventHandler> _logger;

        public DesignationCreatedEventHandler(
            ILogger<DesignationCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DesignationCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
