using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberTel
{
    public int SN { get; set; }

    public string Tel { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
