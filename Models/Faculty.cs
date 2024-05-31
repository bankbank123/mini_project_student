using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public string FacultyName { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
