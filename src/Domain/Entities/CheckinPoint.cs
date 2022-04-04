using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class CheckinPoint : AuditableEntity, IHasDomainEvent, IAuditTrial
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Device>? Devices { get; set; }=new HashSet<Device>();
    public int? SiteId { get; set; }
    public virtual Site? Site { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();
}
