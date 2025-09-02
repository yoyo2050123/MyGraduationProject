using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Member
{

    public string MemberID { get; set; } = null!;

    public string Name { get; set; } = null!;
    
    public string Tel { get; set; } = null!;

    public byte PlanID { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public virtual ICollection<LearnRecordTable>? LearnRecordTable { get; set; } 

    public virtual ICollection<MemberAccount>? MemberAccount { get; set; } 
    public virtual ICollection<MemberPlan>? MemberPlan { get; set; }  

    public virtual ICollection<MemberTel>? MemberTel { get; set; } 

    public virtual ICollection<Note>? Note { get; set; } 

    public virtual ICollection<PaymentRecord>? PaymentRecord { get; set; } 

    public virtual SubscriptionPlan? Plan { get; set; }

    public virtual ICollection<Record>? Record { get; set; } 

    public virtual ICollection<Role>? Role { get; set; } 
}
