using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class JhighSchool
{
    public int JhighSchoolId { get; set; }

    public string JhighSchoolName { get; set; } = null!;

    public double JhighSchoolGrade { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
