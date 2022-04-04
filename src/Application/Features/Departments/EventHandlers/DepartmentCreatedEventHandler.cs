// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Departments.EventHandlers;

    public class DepartmentCreatedEventHandler : INotificationHandler<DomainEventNotification<DepartmentCreatedEvent>>
    {
        private readonly ILogger<DepartmentCreatedEventHandler> _logger;

        public DepartmentCreatedEventHandler(
            ILogger<DepartmentCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DepartmentCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
