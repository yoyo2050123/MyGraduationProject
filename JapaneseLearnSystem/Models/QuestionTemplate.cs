using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class QuestionTemplate
{
    public string QuestionTemplateID { get; set; } = null!;

    public int WordID { get; set; }

    public string QuestionType { get; set; } = null!;

    public string QuestionTemplate1 { get; set; } = null!;

    public virtual ICollection<QuestionInstance> QuestionInstance { get; set; } = new List<QuestionInstance>();

    public virtual Word Word { get; set; } = null!;
}
