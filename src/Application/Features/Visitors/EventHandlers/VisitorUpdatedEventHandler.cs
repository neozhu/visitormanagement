// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Visitors.Constant;
using CleanArchitecture.Blazor.Application.Services.MessageService;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.EventHandlers;

public class VisitorUpdatedEventHandler : INotificationHandler<DomainEventNotification<UpdatedEvent<Visitor>>>
{
    private readonly IApplicationDbContext _context;
    private readonly SMSMessageService _sms;
    private readonly MailMessageService _mail;
    private readonly ILogger<VisitorUpdatedEventHandler> _logger;

    public VisitorUpdatedEventHandler(
        IApplicationDbContext context,
        SMSMessageService sms,
        MailMessageService mail,
        ILogger<VisitorUpdatedEventHandler> logger
        )
    {
        _context = context;
        _sms = sms;
        _mail = mail;
        _logger = logger;
    }
    public async Task Handle(DomainEventNotification<UpdatedEvent<Visitor>> notification, CancellationToken cancellationToken)
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
                if (visitor.Status == VisitorStatus.PendingApproval || visitor.Status == VisitorStatus.PendingConfirm)
                {
                    var emp = _context.Employees.FirstOrDefault(x => x.Id == visitor.EmployeeId);
                    if(emp is not null)
                    {
                        await _sms.Send(emp.PhoneNumber, new string[] { string.Format(template.Body, visitor.PassCode) }, template.Subject);
                    }
                }
                else
                {
                    await _sms.Send(visitor.PhoneNumber, new string[] { string.Format(template.Body, visitor.PassCode) }, template.Subject);
                }
                
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
                if (visitor.Status == VisitorStatus.PendingApproval || visitor.Status == VisitorStatus.PendingConfirm)
                {
                    var emp = _context.Employees.FirstOrDefault(x => x.Id == visitor.EmployeeId);
                    if (emp is not null)
                    {
                        await _mail.Send(emp.Email, template.Subject, string.Format(template.Body,visitor.PassCode));
                    }
                }
                else
                {
                    await _mail.Send(visitor.Email, template.Subject, string.Format(template.Body, visitor.PassCode));
                }
            }
        }
    }
}
