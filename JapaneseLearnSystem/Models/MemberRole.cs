using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class MemberRole
{
    public string? MemberId { get; set; } = null!;

    public string? RoleId { get; set; } = null!;

    public virtual MemberAccount MemberAccount { get; set; }
    public virtual Role Role { get; set; }
}
