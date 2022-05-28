using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.Core;

namespace CleanArchitecture.Blazor.Application.Services.MessageService;
public class MailMessageService
{
    private readonly ILogger<MailMessageService> _logger;
    private readonly IFluentEmail _fluentEmail;

    public MailMessageService(ILogger<MailMessageService> logger,
        IFluentEmail fluentEmail)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
    }
    public async Task Send(string to,string subject,string body)
    {
        await _fluentEmail.To(to)
                          .Subject(subject)
                          .Body(body).SendAsync();
    }
}
