using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class StudentPrefix
{
    public int StudentPrefixId { get; set; }

    public string StudentPrefixName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
