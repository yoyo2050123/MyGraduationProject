using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Word
{
    public int WordID { get; set; }

    public string Word1 { get; set; } = null!;

    public string PartOfSpeech { get; set; } = null!;

    public string WordTranslate { get; set; } = null!;

    public int JLPTLevelID { get; set; }

    public string? Reading { get; set; }

    public virtual JLPTLevel JLPTLevel { get; set; } = null!;

    public virtual ICollection<QuestionInstance> QuestionInstance { get; set; } = new List<QuestionInstance>();

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();
}
