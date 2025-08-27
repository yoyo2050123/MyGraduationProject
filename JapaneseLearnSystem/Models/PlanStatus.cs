using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class PlanStatus
{
    public byte PlanStatusID { get; set; }

    public string PlanStatusName { get; set; } = null!;

    public virtual ICollection<MemberPlan> MemberPlan { get; set; } = new List<MemberPlan>();
}
