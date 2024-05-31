using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mini_Project.Models;

public partial class MiniProjectDbContext : DbContext
{
    public MiniProjectDbContext()
    {
    }

    public MiniProjectDbContext(DbContextOptions<MiniProjectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressHomeland> AddressHomelands { get; set; }

    public virtual DbSet<AddressPresent> AddressPresents { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<ElementarySchool> ElementarySchools { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<JhighSchool> JhighSchools { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<ShighSchool> ShighSchools { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentPrefix> StudentPrefixes { get; set; }

    public virtual DbSet<ThaiAmphure> ThaiAmphures { get; set; }

    public virtual DbSet<ThaiGeography> ThaiGeographies { get; set; }

    public virtual DbSet<ThaiProvince> ThaiProvinces { get; set; }

    public virtual DbSet<ThaiTambon> ThaiTambons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-7R12NSA\\SQLEXPRESS01;Database=MiniProjectDB;Trusted_Connection=True;TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressHomeland>(entity =>
        {
            entity.HasKey(e => e.AddressHomelandId).HasName("PK__AddressH__48D86B2C81D737E3");

            entity.Property(e => e.AddressHomelandId).HasColumnName("AddressHomelandID");
            entity.Property(e => e.AddressHomeland1)
                .HasMaxLength(255)
                .HasColumnName("AddressHomeland");
            entity.Property(e => e.AddressHomelandAmphureId).HasColumnName("AddressHomeland_AmphureID");
            entity.Property(e => e.AddressHomelandProvinceId).HasColumnName("AddressHomeland_ProvinceID");
            entity.Property(e => e.AddressHomelandTambonId).HasColumnName("AddressHomeland_TambonID");

            entity.HasOne(d => d.AddressHomelandAmphure).WithMany(p => p.AddressHomelands)
                .HasForeignKey(d => d.AddressHomelandAmphureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressHo__Addre__46B27FE2");

            entity.HasOne(d => d.AddressHomelandProvince).WithMany(p => p.AddressHomelands)
                .HasForeignKey(d => d.AddressHomelandProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressHo__Addre__45BE5BA9");

            entity.HasOne(d => d.AddressHomelandTambon).WithMany(p => p.AddressHomelands)
                .HasForeignKey(d => d.AddressHomelandTambonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressHo__Addre__47A6A41B");
        });

        modelBuilder.Entity<AddressPresent>(entity =>
        {
            entity.HasKey(e => e.AddressPresentId).HasName("PK__AddressP__233E8082E4F98EE6");

            entity.Property(e => e.AddressPresentId).HasColumnName("AddressPresentID");
            entity.Property(e => e.AddressPresent1)
                .HasMaxLength(255)
                .HasColumnName("AddressPresent");
            entity.Property(e => e.AddressPresentAmphureId).HasColumnName("AddressPresent_AmphureID");
            entity.Property(e => e.AddressPresentProvinceId).HasColumnName("AddressPresent_ProvinceID");
            entity.Property(e => e.AddressPresentTambonId).HasColumnName("AddressPresent_TambonID");

            entity.HasOne(d => d.AddressPresentAmphure).WithMany(p => p.AddressPresents)
                .HasForeignKey(d => d.AddressPresentAmphureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressPr__Addre__41EDCAC5");

            entity.HasOne(d => d.AddressPresentProvince).WithMany(p => p.AddressPresents)
                .HasForeignKey(d => d.AddressPresentProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressPr__Addre__40F9A68C");

            entity.HasOne(d => d.AddressPresentTambon).WithMany(p => p.AddressPresents)
                .HasForeignKey(d => d.AddressPresentTambonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressPr__Addre__42E1EEFE");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FA5B0954784");

            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Branches)
                .HasForeignKey(d => d.FacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Branches__Facult__02FC7413");
        });

        modelBuilder.Entity<ElementarySchool>(entity =>
        {
            entity.HasKey(e => e.ElementarySchoolId).HasName("PK__Elementa__9D31991AB1289C03");

            entity.Property(e => e.ElementarySchoolId).HasColumnName("ElementarySchoolID");
            entity.Property(e => e.ElementarySchoolName).HasMaxLength(100);
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__Facultie__306F636EEDA69142");

            entity.Property(e => e.FacultyId).HasColumnName("FacultyID");
            entity.Property(e => e.FacultyName).HasMaxLength(255);
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.FamilyId).HasName("PK__Families__41D82F4B500A4582");

            entity.Property(e => e.FamilyId).HasColumnName("FamilyID");
            entity.Property(e => e.FamilyRole).HasMaxLength(100);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Families)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Families__Studen__76619304");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Genders__4E24E817A25DC0D7");

            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.GenderName).HasMaxLength(50);
        });

        modelBuilder.Entity<JhighSchool>(entity =>
        {
            entity.HasKey(e => e.JhighSchoolId).HasName("PK__JHighSch__5A04B9CA80F65519");

            entity.ToTable("JHighSchools");

            entity.Property(e => e.JhighSchoolId).HasColumnName("JHighSchoolID");
            entity.Property(e => e.JhighSchoolGrade).HasColumnName("JHighSchoolGrade");
            entity.Property(e => e.JhighSchoolName)
                .HasMaxLength(255)
                .HasColumnName("JHighSchoolName");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("PK__Majors__D5B8BFB11368F04D");

            entity.Property(e => e.MajorId).HasColumnName("MajorID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.MajorName).HasMaxLength(255);

            entity.HasOne(d => d.Branch).WithMany(p => p.Majors)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Majors__BranchID__05D8E0BE");
        });

        modelBuilder.Entity<ShighSchool>(entity =>
        {
            entity.HasKey(e => e.ShighSchoolId).HasName("PK__SHighSch__1A95C747E4456C06");

            entity.ToTable("SHighSchools");

            entity.Property(e => e.ShighSchoolId).HasColumnName("SHighSchoolID");
            entity.Property(e => e.ShighSchoolGrade).HasColumnName("SHighSchoolGrade");
            entity.Property(e => e.ShighSchoolName)
                .HasMaxLength(255)
                .HasColumnName("SHighSchoolName");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A7939B19BFD");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudentAddressHomelandId).HasColumnName("Student_AddressHomelandID");
            entity.Property(e => e.StudentAddressPresentId).HasColumnName("Student_AddressPresentID");
            entity.Property(e => e.StudentAge).HasColumnName("Student_Age");
            entity.Property(e => e.StudentBranchId).HasColumnName("Student_BranchID");
            entity.Property(e => e.StudentElementarySchoolId).HasColumnName("Student_ElementarySchoolID");
            entity.Property(e => e.StudentFacultyId).HasColumnName("Student_FacultyID");
            entity.Property(e => e.StudentFirstName)
                .HasMaxLength(255)
                .HasColumnName("Student_FirstName");
            entity.Property(e => e.StudentGenderId).HasColumnName("Student_GenderID");
            entity.Property(e => e.StudentJhighSchoolId).HasColumnName("Student_JHighSchoolID");
            entity.Property(e => e.StudentLastName)
                .HasMaxLength(255)
                .HasColumnName("Student_LastName");
            entity.Property(e => e.StudentMajorId).HasColumnName("Student_MajorID");
            entity.Property(e => e.StudentMiddleName)
                .HasMaxLength(255)
                .HasColumnName("Student_MiddleName");
            entity.Property(e => e.StudentPrefixId).HasColumnName("Student_PrefixID");
            entity.Property(e => e.StudentShighSchoolId).HasColumnName("Student_SHighSchoolID");

            entity.HasOne(d => d.StudentAddressHomeland).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentAddressHomelandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6CD828CA");

            entity.HasOne(d => d.StudentAddressPresent).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentAddressPresentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6BE40491");

            entity.HasOne(d => d.StudentBranch).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentBranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__681373AD");

            entity.HasOne(d => d.StudentElementarySchool).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentElementarySchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6DCC4D03");

            entity.HasOne(d => d.StudentFaculty).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentFacultyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__671F4F74");

            entity.HasOne(d => d.StudentGender).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentGenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6AEFE058");

            entity.HasOne(d => d.StudentJhighSchool).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentJhighSchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6EC0713C");

            entity.HasOne(d => d.StudentMajor).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentMajorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__690797E6");

            entity.HasOne(d => d.StudentPrefix).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentPrefixId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__69FBBC1F");

            entity.HasOne(d => d.StudentShighSchool).WithMany(p => p.Students)
                .HasForeignKey(d => d.StudentShighSchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Students__Studen__6FB49575");
        });

        modelBuilder.Entity<StudentPrefix>(entity =>
        {
            entity.HasKey(e => e.StudentPrefixId).HasName("PK__StudentP__51ADFB60F1707D97");

            entity.ToTable("StudentPrefix");

            entity.Property(e => e.StudentPrefixId).HasColumnName("StudentPrefixID");
            entity.Property(e => e.StudentPrefixName).HasMaxLength(50);
        });

        modelBuilder.Entity<ThaiAmphure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__thai_amp__3213E83F5B397257");

            entity.ToTable("thai_amphures");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.NameEn)
                .HasMaxLength(150)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(150)
                .HasColumnName("name_th");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");

            entity.HasOne(d => d.Province).WithMany(p => p.ThaiAmphures)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__thai_amph__provi__3B40CD36");
        });

        modelBuilder.Entity<ThaiGeography>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__thai_geo__3213E83FFD1C1996");

            entity.ToTable("thai_geographies");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ThaiProvince>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__thai_pro__3213E83F4A9E69CE");

            entity.ToTable("thai_provinces");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GeographyId).HasColumnName("geography_id");
            entity.Property(e => e.NameEn)
                .HasMaxLength(150)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(150)
                .HasColumnName("name_th");

            entity.HasOne(d => d.Geography).WithMany(p => p.ThaiProvinces)
                .HasForeignKey(d => d.GeographyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__thai_prov__geogr__3587F3E0");
        });

        modelBuilder.Entity<ThaiTambon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__thai_tam__3213E83F6D1CB12C");

            entity.ToTable("thai_tambons");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AmphureId).HasColumnName("amphure_id");
            entity.Property(e => e.NameEn)
                .HasMaxLength(150)
                .HasColumnName("name_en");
            entity.Property(e => e.NameTh)
                .HasMaxLength(150)
                .HasColumnName("name_th");
            entity.Property(e => e.ZipCode).HasColumnName("zip_code");

            entity.HasOne(d => d.Amphure).WithMany(p => p.ThaiTambons)
                .HasForeignKey(d => d.AmphureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__thai_tamb__amphu__3E1D39E1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
