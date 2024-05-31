using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class ElementarySchool
{
    public int ElementarySchoolId { get; set; }

    public string ElementarySchoolName { get; set; } = null!;

    public double ElementarySchoolGrade { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
