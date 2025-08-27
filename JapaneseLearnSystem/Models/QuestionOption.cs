using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class QuestionOption
{
    public string QuestionInstanceID { get; set; } = null!;

    public string OptionID { get; set; } = null!;

    public string OptionContent { get; set; } = null!;

    public virtual QuestionInstance QuestionInstance { get; set; } = null!;
}
