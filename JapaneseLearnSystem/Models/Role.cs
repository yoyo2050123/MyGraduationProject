using System;
using System.Collections.Generic;

namespace JapaneseLearnSystem.Models;

public partial class Role
{
    public string RoleID { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Member> Member { get; set; } = new List<Member>();

    public virtual ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
}
