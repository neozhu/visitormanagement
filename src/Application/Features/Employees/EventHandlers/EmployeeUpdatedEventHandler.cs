// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Employees.EventHandlers;

    public class EmployeeUpdatedEventHandler : INotificationHandler<DomainEventNotification<EmployeeUpdatedEvent>>
    {
        private readonly ILogger<EmployeeUpdatedEventHandler> _logger;

        public EmployeeUpdatedEventHandler(
            ILogger<EmployeeUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<EmployeeUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
