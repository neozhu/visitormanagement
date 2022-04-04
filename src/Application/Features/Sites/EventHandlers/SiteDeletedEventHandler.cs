// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Sites.EventHandlers;

    public class SiteDeletedEventHandler : INotificationHandler<DomainEventNotification<SiteDeletedEvent>>
    {
        private readonly ILogger<SiteDeletedEventHandler> _logger;

        public SiteDeletedEventHandler(
            ILogger<SiteDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<SiteDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
