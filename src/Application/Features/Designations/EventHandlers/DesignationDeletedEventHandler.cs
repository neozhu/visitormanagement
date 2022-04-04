// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Designations.EventHandlers;

    public class DesignationDeletedEventHandler : INotificationHandler<DomainEventNotification<DesignationDeletedEvent>>
    {
        private readonly ILogger<DesignationDeletedEventHandler> _logger;

        public DesignationDeletedEventHandler(
            ILogger<DesignationDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DesignationDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
