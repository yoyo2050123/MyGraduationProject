using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class LearnRecordTable
{
    public string RecordID { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public int LearnedWordCount { get; set; }

    public int TotalAnswers { get; set; }

    public int CorrectAnswers { get; set; }

    public decimal Accuracy { get; set; }

    public DateTime AnswerTime { get; set; }

    public virtual Member Member { get; set; } = null!;
}
