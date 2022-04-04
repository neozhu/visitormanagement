// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.CheckinPoints.EventHandlers;

    public class CheckinPointDeletedEventHandler : INotificationHandler<DomainEventNotification<CheckinPointDeletedEvent>>
    {
        private readonly ILogger<CheckinPointDeletedEventHandler> _logger;

        public CheckinPointDeletedEventHandler(
            ILogger<CheckinPointDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<CheckinPointDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
