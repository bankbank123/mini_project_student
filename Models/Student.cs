using System;
using System.Collections.Generic;

namespace Mini_Project.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }

        public string StudentFirstName { get; set; } = null!;

        public string? StudentMiddleName { get; set; }

        public string StudentLastName { get; set; } = null!;

        public int StudentAge { get; set; }

        public int StudentPrefixId { get; set; }

        public int StudentGenderId { get; set; }

        public int StudentAddressPresentId { get; set; }

        public int StudentAddressHomelandId { get; set; }

        public int StudentElementarySchoolId { get; set; }

        public int StudentJhighSchoolId { get; set; }

        public int StudentShighSchoolId { get; set; }

        public int StudentFacultyId { get; set; }

        public int StudentBranchId { get; set; }

        public int? StudentMajorId { get; set; } // Nullable StudentMajorId

        public virtual ICollection<Family> Families { get; set; } = new List<Family>();

        public virtual AddressHomeland StudentAddressHomeland { get; set; } = null!;

        public virtual AddressPresent StudentAddressPresent { get; set; } = null!;

        public virtual Branch StudentBranch { get; set; } = null!;

        public virtual ElementarySchool StudentElementarySchool { get; set; } = null!;

        public virtual Faculty StudentFaculty { get; set; } = null!;

        public virtual Gender StudentGender { get; set; } = null!;

        public virtual JhighSchool StudentJhighSchool { get; set; } = null!;

        public virtual Major? StudentMajor { get; set; } // Nullable StudentMajor

        public virtual StudentPrefix StudentPrefix { get; set; } = null!;

        public virtual ShighSchool StudentShighSchool { get; set; } = null!;
    }
}
