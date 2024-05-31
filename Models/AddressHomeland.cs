using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class AddressHomeland
{
    public int AddressHomelandId { get; set; }

    public string AddressHomeland1 { get; set; } = null!;

    public int AddressHomelandProvinceId { get; set; }

    public int AddressHomelandAmphureId { get; set; }

    public int AddressHomelandTambonId { get; set; }

    public virtual ThaiAmphure AddressHomelandAmphure { get; set; } = null!;

    public virtual ThaiProvince AddressHomelandProvince { get; set; } = null!;

    public virtual ThaiTambon AddressHomelandTambon { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
