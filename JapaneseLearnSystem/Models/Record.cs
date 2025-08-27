using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Record
{
    public string MemberID { get; set; } = null!;

    public string QuestionInstanceID { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public DateTime RecordTime { get; set; }

    public DateTime AnswerTime { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual QuestionInstance QuestionInstance { get; set; } = null!;
}
