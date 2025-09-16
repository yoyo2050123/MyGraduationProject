using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class QuestionTemplate
{
    public string QuestionTemplateID { get; set; } = null!;

    public string QuestionType { get; set; } = null!;

    public string QuestionTemplateText { get; set; } = null!;

    public virtual ICollection<QuestionInstance> QuestionInstance { get; set; } = new List<QuestionInstance>();
}
