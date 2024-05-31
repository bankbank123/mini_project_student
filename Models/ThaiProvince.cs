using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class ThaiProvince
{
    public int Id { get; set; }

    public string NameTh { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public int GeographyId { get; set; }

    public virtual ICollection<AddressHomeland> AddressHomelands { get; set; } = new List<AddressHomeland>();

    public virtual ICollection<AddressPresent> AddressPresents { get; set; } = new List<AddressPresent>();

    public virtual ThaiGeography Geography { get; set; } = null!;

    public virtual ICollection<ThaiAmphure> ThaiAmphures { get; set; } = new List<ThaiAmphure>();
}
