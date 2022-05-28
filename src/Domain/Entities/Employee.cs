using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Blazor.Domain.Entities.Tenant;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Employee : AuditableEntity, IHasDomainEvent, IAuditTrial, IMustHaveTenant
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public int? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }
    public int? DesignationId { get; set; }
    public virtual Designation? Designation { get; set; }
    public string? About { get; set; }
    public string? Avatar { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new();

    public string? RelatedAccountId { get; set; }

    public int? SiteId { get; set; }
    public virtual Site? Site { get; set; }
}
