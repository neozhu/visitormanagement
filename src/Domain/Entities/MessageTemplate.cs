using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Blazor.Domain.Entities.Tenant;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class MessageTemplate : AuditableEntity, IHasDomainEvent, IAuditTrial, IMustHaveTenant
{
    public int Id { get; set; }
    public MessageType MessageType { get; set; } = MessageType.Email;
    public string? ForStatus { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();
    public int? SiteId { get; set; }
    public virtual Site? Site { get; set; }

    public string? Description { get; set; }
}
