// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.EventHandlers;

    public class MessageTemplateCreatedEventHandler : INotificationHandler<DomainEventNotification<CreatedEvent<MessageTemplate>>>
    {
        private readonly ILogger<MessageTemplateCreatedEventHandler> _logger;

        public MessageTemplateCreatedEventHandler(
            ILogger<MessageTemplateCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<CreatedEvent<MessageTemplate>> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
