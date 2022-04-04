// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.EventHandlers;

    public class CheckinPointUpdatedEventHandler : INotificationHandler<DomainEventNotification<CheckinPointUpdatedEvent>>
    {
        private readonly ILogger<CheckinPointUpdatedEventHandler> _logger;

        public CheckinPointUpdatedEventHandler(
            ILogger<CheckinPointUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<CheckinPointUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
