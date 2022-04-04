// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.EventHandlers;

    public class VisitorUpdatedEventHandler : INotificationHandler<DomainEventNotification<VisitorUpdatedEvent>>
    {
        private readonly ILogger<VisitorUpdatedEventHandler> _logger;

        public VisitorUpdatedEventHandler(
            ILogger<VisitorUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<VisitorUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
