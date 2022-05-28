// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;
using CleanArchitecture.Blazor.Application.Services.MessageService;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.EventHandlers;

public class VisitorCreatedEventHandler : INotificationHandler<DomainEventNotification<CreatedEvent<Visitor>>>
{
    private readonly IApplicationDbContext _context;
    private readonly SMSMessageService _sms;
    private readonly MailMessageService _mail;
    private readonly ILogger<VisitorCreatedEventHandler> _logger;

    public VisitorCreatedEventHandler(
        IApplicationDbContext context,
        SMSMessageService sms,
        MailMessageService mail,
        ILogger<VisitorCreatedEventHandler> logger
        )
    {
        _context = context;
        _sms = sms;
        _mail = mail;
        _logger = logger;
    }
    public async Task Handle(DomainEventNotification<CreatedEvent<Visitor>> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var visitor = domainEvent.Entity;
        if (visitor.PhoneNumber != null)
        {
            var template = await _context.MessageTemplates.FirstOrDefaultAsync(x =>
                  x.SiteId == visitor.SiteId &&
                  x.MessageType == MessageType.Sms &&
                  x.ForStatus == visitor.Status, cancellationToken);
            if (template is not null)
            {
                
                await _sms.Send(visitor.PhoneNumber, new string[] { String.Format(template.Body,visitor.PassCode) }, template.Subject);
            }
        }
        if (visitor.Email != null)
        {
            var template = await _context.MessageTemplates.FirstOrDefaultAsync(x =>
                  x.SiteId == visitor.SiteId &&
                  x.MessageType == MessageType.Email &&
                  x.ForStatus == visitor.Status, cancellationToken);
            if (template is not null)
            {
                await _mail.Send(visitor.Email, template.Subject, string.Format(template.Body, visitor.PassCode));
            }
        }
    }
}
