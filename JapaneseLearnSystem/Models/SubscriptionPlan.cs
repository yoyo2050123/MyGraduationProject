using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class SubscriptionPlan
{
    public byte PlanID { get; set; }

    public string PlanName { get; set; } = null!;

    public string FeeInfo { get; set; } = null!;

    public int? DailyQuestionLimit { get; set; }

    public int? LearnedWordLimit { get; set; }

    public int? FavoriteLimit { get; set; }

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();

    public virtual ICollection<MemberPlan> MemberPlan { get; set; } = new List<MemberPlan>();

    public virtual ICollection<PaymentRecord> PaymentRecord { get; set; } = new List<PaymentRecord>();
}
