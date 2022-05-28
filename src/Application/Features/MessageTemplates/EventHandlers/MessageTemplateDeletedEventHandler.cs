// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.EventHandlers;

    public class MessageTemplateDeletedEventHandler : INotificationHandler<DomainEventNotification<DeletedEvent<MessageTemplate>>>
    {
        private readonly ILogger<MessageTemplateDeletedEventHandler> _logger;

        public MessageTemplateDeletedEventHandler(
            ILogger<MessageTemplateDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<DeletedEvent<MessageTemplate>> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
