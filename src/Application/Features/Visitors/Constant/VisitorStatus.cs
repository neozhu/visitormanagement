using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Blazor.Application.Features.Visitors.Constant;
public static class VisitorStatus
{
    public static string PendingVisitor => "Pending Visitor";
    public static string PendingApproval => "Pending Approval";
    public static string PendingChecking => "Pending Checking";
    public static string PendingCheckin => "Pending Check-in";
    public static string PendingConfirm => "Pending Confirm";
    public static string PendingCheckout => "Pending Check-out";
    public static string PendingFeedback => "Pending Feedback";
    public static string Finished => "Finished";
    public static string Canceled => "Canceled";
}
