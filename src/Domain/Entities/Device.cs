using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Device : AuditableEntity, IHasDomainEvent, IAuditTrial
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IPAddress { get; set; }
    public string? Parameters { get; set; }
    public string? Status { get; set; }
    public int? CheckinPointId{get;set;}
    public virtual CheckinPoint? CheckinPoint { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new();
}
