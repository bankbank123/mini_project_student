using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class ShighSchool
{
    public int ShighSchoolId { get; set; }

    public string ShighSchoolName { get; set; } = null!;

    public double ShighSchoolGrade { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
