using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class JLPTLevel
{
    public int JLPTLevelID { get; set; }

    public string JLPTLevelName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();

    public virtual ICollection<Word> Word { get; set; } = new List<Word>();
}
