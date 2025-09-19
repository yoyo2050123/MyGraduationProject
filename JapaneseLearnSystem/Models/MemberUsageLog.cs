using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberUsageLog
{
    public string UsageLogID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public DateOnly UsageLogDate { get; set; }

    public int? WordCount { get; set; }

    public int? NoteCount { get; set; }

    public int? QuestionCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Member Member { get; set; } = null!;
}
