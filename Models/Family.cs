using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class Family
{
    public int FamilyId { get; set; }

    public string FamilyRole { get; set; } = null!;

    public int StudentId { get; set; }

    public virtual Student Student { get; set; } = null!;
}
