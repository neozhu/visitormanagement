// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.MessageTemplates.DTOs;


public class MessageTemplateDto:IMapFrom<MessageTemplate>
{
    public void Mapping(Profile profile)
    {
        profile.CreateMap<MessageTemplate, MessageTemplateDto>(MemberList.None)
               .ForMember(x => x.CompanyName, y => y.MapFrom(z => z.Site.CompanyName))
               .ForMember(x => x.SiteName, y => y.MapFrom(z => z.Site.Name));
        profile.CreateMap<MessageTemplateDto, MessageTemplate>(MemberList.None);
    }
    public int Id { get; set; }
    public MessageType MessageType { get; set; } = MessageType.Email;
    public string? ForStatus { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public int? SiteId { get; set; }
    public string? SiteName { get; set; }
    public string? CompanyName { get; set; }
    public string? Description { get; set; }

}

