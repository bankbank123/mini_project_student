using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class ThaiGeography
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ThaiProvince> ThaiProvinces { get; set; } = new List<ThaiProvince>();
}
