using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberRole
{
    public string MemberID { get; set; } = null!;

    public string RoleID { get; set; } = null!;

    public string? Empty { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
