// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ApprovalHistories.DTOs;


public class ApprovalHistoryDto:IMapFrom<ApprovalHistory>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApprovalHistory, ApprovalHistoryDto>().ReverseMap();
    }
    public int Id { get; set; }
    public int? VisitorId { get; set; }
    public string? Outcome { get; set; }
    public string? Comment { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ProcessingDate { get; set; }

}

