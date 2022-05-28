using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Domain;
public enum MessageType
{
    [Description("Email")]
    Email,
    [Description("Sms")]
    Sms
}
