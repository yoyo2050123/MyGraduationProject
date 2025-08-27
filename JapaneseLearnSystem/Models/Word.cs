using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Word
{
    public int WordID { get; set; }

    public string Word1 { get; set; } = null!;

    public string PartOfSpeech { get; set; } = null!;

    public string WordTranslate { get; set; } = null!;

    public int JPLTLevelID { get; set; }

    public virtual JPLTLevel JPLTLevel { get; set; } = null!;

    public virtual ICollection<QuestionTemplate> QuestionTemplate { get; set; } = new List<QuestionTemplate>();

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();
}
