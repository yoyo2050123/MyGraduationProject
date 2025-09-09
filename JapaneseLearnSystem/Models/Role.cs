using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Role
{
    public string RoleID { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<MemberRole> MemberRole { get; set; } = new List<MemberRole>();
}
