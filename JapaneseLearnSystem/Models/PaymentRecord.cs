using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class PaymentRecord
{
    public string PaymentID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public byte PlanID { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string Method { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual SubscriptionPlan Plan { get; set; } = null!;
}
