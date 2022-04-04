using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Site : AuditableEntity, IHasDomainEvent, IAuditTrial
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public virtual ICollection<CheckinPoint> CheckinPoints { get; set; }=new HashSet<CheckinPoint>();

    public List<DomainEvent> DomainEvents { get; set; } = new();
}
