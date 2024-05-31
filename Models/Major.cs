using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class Major
{
    public int MajorId { get; set; }

    public string MajorName { get; set; } = null!;

    public int BranchId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
