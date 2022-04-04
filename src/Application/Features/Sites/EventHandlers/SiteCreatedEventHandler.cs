// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Sites.EventHandlers;

    public class SiteCreatedEventHandler : INotificationHandler<DomainEventNotification<SiteCreatedEvent>>
    {
        private readonly ILogger<SiteCreatedEventHandler> _logger;

        public SiteCreatedEventHandler(
            ILogger<SiteCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<SiteCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
