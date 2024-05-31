using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public int FacultyId { get; set; }

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
