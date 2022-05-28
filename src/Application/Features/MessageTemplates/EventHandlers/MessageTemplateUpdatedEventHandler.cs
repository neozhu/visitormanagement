// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.EventHandlers;

    public class MessageTemplateUpdatedEventHandler : INotificationHandler<DomainEventNotification<UpdatedEvent<MessageTemplate>>>
    {
        private readonly ILogger<MessageTemplateUpdatedEventHandler> _logger;

        public MessageTemplateUpdatedEventHandler(
            ILogger<MessageTemplateUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DomainEventNotification<UpdatedEvent<MessageTemplate>> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
