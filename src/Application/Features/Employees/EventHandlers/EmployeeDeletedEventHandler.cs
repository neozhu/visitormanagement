// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Employees.EventHandlers;

    public class EmployeeDeletedEventHandler : INotificationHandler<DomainEventNotification<EmployeeDeletedEvent>>
    {
        private readonly ILogger<EmployeeDeletedEventHandler> _logger;

        public EmployeeDeletedEventHandler(
            ILogger<EmployeeDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<EmployeeDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
