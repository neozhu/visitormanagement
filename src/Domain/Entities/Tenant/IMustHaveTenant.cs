using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain.Entities.Tenant;
public interface IMustHaveTenant
{
    int? SiteId { get; set; }
    Site? Site { get; set; }
}
