using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.DTOs;
public class VisitorStatusSumarryDto
{
    public string? Status { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CompanyName { get; set; }
    public string? PhoneNumber { get; set; }
}
