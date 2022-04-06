// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;


public class VisitorDto:IMapFrom<Visitor>
{
    public void Mapping(Profile profile)
    {
        
            profile.CreateMap<Visitor, VisitorDto>()
               .ForMember(x => x.Designation, s => s.MapFrom(y => y.Designation.Name))
               .ForMember(x => x.Employee, s => s.MapFrom(y => y.Employee.Name))
               .ForMember(x => x.Address, s => s.MapFrom(y => y.Site.Address))
               .ForMember(x => x.EmployeeDesignation, s => s.MapFrom(y => y.Employee.Designation.Name));
            profile.CreateMap<VisitorDto, Visitor>(MemberList.None);
        
    }
    public int Id { get; set; }
    public string? PassCode { get; set; }
    public string? QrCode { get; set; }
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
    public string? Designation { get; set; }
    public int? EmployeeId { get; set; }
    public string? Employee { get; set; }
    public string? EmployeeDesignation { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public TimeSpan? ExpectedTime { get; set; }
    public int? SiteId { get; set; }
    public string? Avatar { get; set; }
    public string? TripCode { get; set; }
    public string? HealthCode { get; set; }
    public string? NucleicAcidTestReport { get; set; }
    public bool? PrivacyPolicy { get; set; }
    public bool? Promise { get; set; }
    public string? Status { get; set; }
    public bool? Apppoved { get; set; }
    public string? ApprovalOutcome { get; set; }

}

