// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.ApprovalHistories.DTOs;
using CleanArchitecture.Blazor.Application.Features.VisitorHistories.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;


public class VisitorDto : IMapFrom<Visitor>
{
    public void Mapping(Profile profile)
    {

        profile.CreateMap<Visitor, VisitorDto>()
           .ForMember(x => x.Designation, s => s.MapFrom(y => y.Designation.Name))
           .ForMember(x => x.Employee, s => s.MapFrom(y => y.Employee.Name))
           .ForMember(x => x.Address, s => s.MapFrom(y => $"{y.Site.Name} - {y.Site.Address}"))
           .ForMember(x => x.EmployeeDesignation, s => s.MapFrom(y => y.Employee.Designation.Name))
           .ForMember(x => x.Companions, s => s.MapFrom(y => y.Companions.Select(x => new CompanionDto()
           {
               Name = x.Name,
               IdentificationNo = x.IdentificationNo,
               CheckinDateTime = x.CheckinDateTime,
               CheckoutDateTime = x.CheckoutDateTime,
               Description = x.Description,
               HealthCode = x.HealthCode,
               Id = x.Id,
               NucleicAcidTestReport = x.NucleicAcidTestReport,
               QrCode = x.QrCode,
               TripCode = x.TripCode,
               VisitorId = x.VisitorId
           }).ToList()))
           .ForMember(x => x.ApprovalHistories, s => s.MapFrom(y => y.ApprovalHistories.Select(x => new ApprovalHistoryDto()
           {
               Id = x.Id,
               ApprovedBy = x.ApprovedBy,
               Comment = x.Comment,
               Outcome = x.Outcome,
               ProcessingDate = x.ProcessingDate,
               VisitorId = x.VisitorId
           }).ToList()))
           .ForMember(x => x.VisitorHistories, s => s.MapFrom(y => y.VisitorHistories.Select(x => new VisitorHistoryDto()
           {
               Id = x.Id,
               CheckinPointId = x.CheckinPointId,
               CheckinPoint=$"{x.CheckinPoint.Site.Name} - {x.CheckinPoint.Name}",
               Comment = x.Comment,
               Attachments = x.Attachments,
               Stage = x.Stage,
               Temperature = x.Temperature,
               TransitDateTime = x.TransitDateTime,
               VisitorId = x.VisitorId
           }).ToList()))
           .ForMember(x => x.CompanionCount, opt => opt.Ignore())
           .ForMember(x => x._processing, opt => opt.Ignore());
        profile.CreateMap<VisitorDto, Visitor>(MemberList.None)
               .ForMember(x => x.Designation, s => s.Ignore())
              .ForMember(x => x.Employee, s => s.Ignore())
              .ForMember(x => x.Companions, s => s.Ignore())
              .ForMember(x => x.ApprovalHistories, s => s.Ignore());

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
    public string? ApprovalComment { get; set; }
    public int CompanionCount => Companions.Count;
    public int? SurveyResponseValue { get; set; }
    public List<CompanionDto> Companions { get; set; } = new();
    public List<VisitorHistoryDto> VisitorHistories { get; set; } = new();
    public List<ApprovalHistoryDto> ApprovalHistories { get; set; } = new();

    public bool _processing { get; set; }
}

