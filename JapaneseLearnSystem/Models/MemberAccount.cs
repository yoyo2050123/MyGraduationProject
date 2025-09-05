using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberAccount
{
    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    
    public string MemberID { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
