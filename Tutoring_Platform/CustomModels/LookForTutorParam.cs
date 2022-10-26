namespace Tutoring_Platform.CustomModels
{
    public class StudentParam
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Postal { get; set; }
        public string? Province { get; set; }
        public string? School { get; set; }
        public string? Field { get; set; }
        public string? Program { get; set; }
        public string? Semester { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }
    }
    public class LookForTutorParam
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
    }
    public class SearchTutorsReturn
    {
        public string? tutorId { get; set; }
        public string? studId { get; set; }
        public string? Name { get; set; }
        public string? School { get; set; }
        public string? Status { get; set; }
        public string? Program { get; set; }
        public string? Wage { get; set; }
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
    }
    public class sendTutorRequestParam
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public int? tutorId { get; set; }
        public int? studId { get; set; }
    }
    public class DisplayRequestsReturn
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public string? Name { get; set; }
        public string? Semester { get; set; }
        public string? School { get; set; }
        public string? Program { get; set; }
    }
}
