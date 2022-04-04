using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class VisitorHistory: AuditableEntity
{
    public int Id { get; set; }
    public int? VisitorId { get; set; }
    public virtual Visitor? Visitor { get; set; }
    public int? CheckinPointId { get; set; }
    public virtual CheckinPoint? CheckinPoint { get; set; }
    public string? Stage { get; set; }
    public string? Comment { get; set; }
    public DateTime? TransitDateTime { get; set; }
    public decimal? Temperature { get; set; }
    public string? Photo { get; set; }

}
