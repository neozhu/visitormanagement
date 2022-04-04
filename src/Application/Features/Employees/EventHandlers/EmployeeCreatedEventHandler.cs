// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Employees.EventHandlers;

    public class EmployeeCreatedEventHandler : INotificationHandler<DomainEventNotification<EmployeeCreatedEvent>>
    {
        private readonly ILogger<EmployeeCreatedEventHandler> _logger;

        public EmployeeCreatedEventHandler(
            ILogger<EmployeeCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<EmployeeCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
