using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JapaneseLearnSystem.Models;

public partial class Member
{
    public string MemberID { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Tel { get; set; } = null!;

    public byte PlanID { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly Birthday { get; set; }

    public virtual ICollection<LearnRecordTable> LearnRecordTable { get; set; } = new List<LearnRecordTable>();

    public virtual ICollection<MemberAccount> MemberAccount { get; set; } = new List<MemberAccount>();

    public virtual ICollection<MemberPlan> MemberPlan { get; set; } = new List<MemberPlan>();

    public virtual ICollection<MemberRole> MemberRole { get; set; } = new List<MemberRole>();

    public virtual ICollection<MemberTel> MemberTel { get; set; } = new List<MemberTel>();

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();

    public virtual ICollection<PaymentRecord> PaymentRecord { get; set; } = new List<PaymentRecord>();

    public virtual SubscriptionPlan? Plan { get; set; }

    public virtual ICollection<Record> Record { get; set; } = new List<Record>();
}
