using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class ApprovalHistory : AuditableEntity, IHasDomainEvent, IAuditTrial
{
    public int Id { get; set; }
    public int? VisitorId { get; set; }
    public virtual Visitor? Visitor { get; set; }
    public string? Outcome { get; set; }
    public string? Comment { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ProcessingDate { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();
}
