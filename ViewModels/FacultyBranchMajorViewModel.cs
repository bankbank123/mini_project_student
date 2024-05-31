namespace Mini_Project.ViewModels
{
    public class FacultyBranchMajorViewModel
    {
        public int FacultyId { get; set; }
        public string? FacultyName { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? MajorId { get; set; }
        public string? MajorName { get; set; }
    }

    public class FacultyBranchMajorEditViewModel
    {
        public int? MajorId { get; set; }
        public string? MajorName { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public int FacultyId { get; set; }
        public string? FacultyName { get; set; }
    }
    public class SearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public IEnumerable<FacultyBranchMajorViewModel> Results { get; set; }
    }
}
