using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberPlan
{
    public string MemberPlanID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public byte PlanID { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string RemarkSource { get; set; } = null!;

    public byte PlanStatusID { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual SubscriptionPlan Plan { get; set; } = null!;

    public virtual PlanStatus PlanStatus { get; set; } = null!;
}
