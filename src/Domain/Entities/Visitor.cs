using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Visitor : AuditableEntity, IHasDomainEvent, IAuditTrial
{
    public int Id { get; set; }
    public string? PassCode { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IdentificationNo { get; set; }
    public string? LicensePlateNumber { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public string? CompanyName { get; set; }
    public string? Purpose { get; set; }
    public string? Comment { get; set; }
    public int? DesignationId { get; set; }
    public virtual Designation? Designation { get; set; }
    public int? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public TimeSpan? ExpectedTime { get; set; }
    public string? Avatar { get; set; }
    public string? TripCode { get; set; }
    public string? HealthCode { get; set; }
    public string? QrCode { get; set; }
    public string? NucleicAcidTestReport { get; set; }
    public bool? PrivacyPolicy { get; set; }
    public bool? Promise { get; set; }
    public string? Status { get; set; }
    public bool? Apppoved { get; set; }
    public string? ApprovalOutcome { get; set; }
    public int? SiteId { get; set; }
    public virtual Site? Site { get; set; }  
    public virtual ICollection<VisitorHistory> VisitorHistories { get; set; }=new HashSet<VisitorHistory>();
    public List<DomainEvent> DomainEvents { get; set; } = new();
}
