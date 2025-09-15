using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Note
{
    public int NoteID { get; set; }

    public string Title { get; set; } = null!;

    public string OriginalArticle { get; set; } = null!;

    public string Translate { get; set; } = null!;

    public int JLPTLevelID { get; set; }

    public string MemberID { get; set; } = null!;

    public string? Reading { get; set; }

    public virtual JLPTLevel JLPTLevel { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<Word> Word { get; set; } = new List<Word>();
}
