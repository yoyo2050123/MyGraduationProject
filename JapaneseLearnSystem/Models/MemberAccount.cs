using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JapaneseLearnSystem.Models;

public partial class MemberAccount
{
    [Required ()]
    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string MemberID { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
