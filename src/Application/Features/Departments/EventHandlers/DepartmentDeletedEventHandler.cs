// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

    public class DepartmentDeletedEventHandler : INotificationHandler<DomainEventNotification<DepartmentDeletedEvent>>
    {
        private readonly ILogger<DepartmentDeletedEventHandler> _logger;

        public DepartmentDeletedEventHandler(
            ILogger<DepartmentDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DepartmentDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
