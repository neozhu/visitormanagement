using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Companion:AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? IdentificationNo { get; set; }
    public string? Description { get; set; }
    public string? TripCode { get; set; }
    public string? HealthCode { get; set; }
    public string? QrCode { get; set; }
    public string? NucleicAcidTestReport { get; set; }
    public int? VisitorId { get; set; }
    public virtual Visitor? Visitor { get; set; }

    public DateTime? CheckinDateTime { get; set; }
    public DateTime? CheckoutDateTime { get; set; }
    
}
