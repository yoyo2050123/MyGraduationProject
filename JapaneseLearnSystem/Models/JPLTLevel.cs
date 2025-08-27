using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class JPLTLevel
{
    public int JPLTLevelID { get; set; }

    public string JPLTLevelName { get; set; } = null!;

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();

    public virtual ICollection<Word> Word { get; set; } = new List<Word>();
}
