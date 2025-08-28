using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearnSystem.Models;

public partial class JLPTLevel
{
    public int JLPTLevelID { get; set; }
    [Display(Name ="JPLT等級")]
    public string JLPTLevelName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Note> Note { get; set; } = new List<Note>();

    public virtual ICollection<Word> Word { get; set; } = new List<Word>();
}
