// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Sites.EventHandlers;

    public class SiteUpdatedEventHandler : INotificationHandler<DomainEventNotification<SiteUpdatedEvent>>
    {
        private readonly ILogger<SiteUpdatedEventHandler> _logger;

        public SiteUpdatedEventHandler(
            ILogger<SiteUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<SiteUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
