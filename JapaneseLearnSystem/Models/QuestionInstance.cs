using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class QuestionInstance
{
    public string QuestionInstanceID { get; set; } = null!;

    public string QuestionTemplateID { get; set; } = null!;

    public int WordID { get; set; }

    public string AnswerOptionID { get; set; } = null!;

    public string QuestionContent { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<QuestionOption> QuestionOption { get; set; } = new List<QuestionOption>();

    public virtual QuestionTemplate QuestionTemplate { get; set; } = null!;

    public virtual ICollection<Record> Record { get; set; } = new List<Record>();

    public virtual Word Word { get; set; } = null!;
}
