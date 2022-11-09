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

    public class TutorParam
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Postal { get; set; }
        public string? Province { get; set; }
        public string? School { get; set; }
        public string? Field { get; set; }
        public string? Program { get; set; }
        public string? Semester { get; set; }
        public string? Wage { get; set; }
        public string[]? Subjects { get; set; }
        public int[]? Days { get; set; }
    }
    public class LookForTutorParam
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public int UserId { get; set; }
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
        public int? Id { get; set; }
        public int? StudId { get; set; }
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public string? Name { get; set; }
        public string? Semester { get; set; }
        public string? School { get; set; }
        public string? Program { get; set; }
    }
    public class DisplayStudRequestsReturn
    {
        public string? Name { get; set; }
        public string? CourseName { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public string? Slot3 { get; set; }
        public string? Slot4 { get; set; }
        public string? Slot5 { get; set; }
        public int? Id1 { get; set; }
        public int? Id2 { get; set; }
        public int? Id3 { get; set; }
        public int? Id4 { get; set; }
        public int? Id5 { get; set; }
    }
    public class SendAppointSlots
    {
        public int? requestId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public string? Slot3 { get; set; }
        public string? Slot4 { get; set; }
        public string? Slot5 { get; set; }
    }
    public class GetConfirmedAppoint
    {
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? Slot { get; set; }
        public int? Id { get; set; }
    }
    public class AddToAppoints
    {
        public int? slotId { get; set; }
        public string? paypal { get; set; }
        public string? zoom { get; set; }
    }

    public class GetAppointments
    {
        public string? Date { get; set; }
        public string? Course { get; set; }
        public string? TutorStud { get; set; }
        public string? Paypal { get; set; }
        public string? Zoom { get; set; }
    }
    public class AskQuery
    {
        public string? UserId { get; set; }
        public string? Query { get; set; }
        public int? QueryId { get; set; }
    }

    public class ReportAccountRequest
    {
        public int? UserId { get; set; }
        public int? AccountId { get; set; }
    }

    public class GetReportAccount
    {
        public string? Name { get; set; }
        public string? By { get; set; }
        public int? AccountId { get; set; }
    }

}
