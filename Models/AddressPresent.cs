using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class AddressPresent
{
    public int AddressPresentId { get; set; }

    public string AddressPresent1 { get; set; } = null!;

    public int AddressPresentProvinceId { get; set; }

    public int AddressPresentAmphureId { get; set; }

    public int AddressPresentTambonId { get; set; }

    public virtual ThaiAmphure AddressPresentAmphure { get; set; } = null!;

    public virtual ThaiProvince AddressPresentProvince { get; set; } = null!;

    public virtual ThaiTambon AddressPresentTambon { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
