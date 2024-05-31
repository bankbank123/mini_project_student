using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_Project.ViewModels
{
    public class StudentFamilyViewModel
    {
        public Student Student { get; set; } = new Student();
        public AddressHomeland AddressHomeland { get; set; } = new AddressHomeland();
        public AddressPresent AddressPresent { get; set; } = new AddressPresent();
        public ElementarySchool ElementarySchool { get; set; } = new ElementarySchool();
        public JhighSchool JhighSchool { get; set; } = new JhighSchool();
        public ShighSchool ShighSchool { get; set; } = new ShighSchool();
        public List<Family> Families { get; set; } = new List<Family>();
        public int ZipCodePresent { get; set; }
        public int ZipCodeHomeland { get; set; }
        public int FamilyNumber { get; set; }
        public bool SameAddress { get; set; }
    }

    public class StudentDetailsViewModel {
        public Student Student { get; set; } = new Student();
        public AddressHomeland AddressHomeland { get; set; } = new AddressHomeland();
        public AddressPresent AddressPresent { get; set; } = new AddressPresent();
        public ElementarySchool ElementarySchool { get; set; } = new ElementarySchool();
        public JhighSchool JhighSchool { get; set; } = new JhighSchool();
        public ShighSchool ShighSchool { get; set; } = new ShighSchool();
        public List<Family> Families { get; set; } = new List<Family>();

        public string AddressHomelandTambon { get; set; } = null!;
        public string AddressHomelandAmphure { get; set; } = null!;
        public string AddressHomelandProvince { get; set; } = null!;
        public int AddressHomelandZipCode { get; set; } 
        public string AddressPresentTambon { get; set; } = null!;
        public string AddressPresentAmphure { get; set; } = null!;
        public string AddressPresentProvince { get; set; } = null!;
        public int AddressPresentZipCode { get; set; } 

    }


}