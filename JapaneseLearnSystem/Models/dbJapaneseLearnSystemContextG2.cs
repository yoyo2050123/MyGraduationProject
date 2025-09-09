using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using JapaneseLearnSystem.Models;

namespace JapaneseLearnSystem.Models;

public partial class dbJapaneseLearnSystemContextG2 : dbJapaneseLearnSystemContext
{
    public dbJapaneseLearnSystemContextG2(DbContextOptions<dbJapaneseLearnSystemContext> options)
        : base(options)
    {
    }
    
}
