using System;
using System.Collections.Generic;

namespace Mini_Project.Models;

public partial class ThaiTambon
{
    public int Id { get; set; }

    public int ZipCode { get; set; }

    public string NameTh { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public int AmphureId { get; set; }

    public virtual ICollection<AddressHomeland> AddressHomelands { get; set; } = new List<AddressHomeland>();

    public virtual ICollection<AddressPresent> AddressPresents { get; set; } = new List<AddressPresent>();

    public virtual ThaiAmphure Amphure { get; set; } = null!;
}
