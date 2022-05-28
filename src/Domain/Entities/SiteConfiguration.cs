using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class SiteConfiguration: AuditableEntity
{
    public int Id { get; set; }
    public int? SiteId { get; set; }
    public virtual Site? Site { get; set; }
    public bool MandatoryHealthQrCode { get; set; } = true;
    public bool MandatoryTripCode { get; set; } = true;
    public bool MandatoryNucleicAcidTestReport { get; set; } = false;
    public string? Parameters { get; set; }
    public string? Description { get; set; }
}
