// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.EventHandlers;

    public class VisitorDeletedEventHandler : INotificationHandler<DomainEventNotification<VisitorDeletedEvent>>
    {
        private readonly ILogger<VisitorDeletedEventHandler> _logger;

        public VisitorDeletedEventHandler(
            ILogger<VisitorDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<VisitorDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
