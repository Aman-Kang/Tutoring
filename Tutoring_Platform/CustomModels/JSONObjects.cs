namespace Tutoring_Platform.CustomModels
{
    // These are custom classes created to have json objects which can be passed among the components and controllers

    /// <summary>
    /// This class holds the profile data of users while passing it as a parameter or while returning it from
    /// controller method
    /// </summary>
    public class ProfileData
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

    /// <summary>
    /// This class holds the Profile data of the tutor while passing it as parameter or returning it from controller
    /// 
    /// </summary>
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

    /// <summary>
    /// This class holds the parameter values when the student searches for the tutor
    /// </summary>
    public class LookForTutorParam
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public int UserId { get; set; }
    }

    /// <summary>
    /// The class that hold the returned list of the searched tutors for the student
    /// </summary>
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

    /// <summary>
    /// When the student sends a tutoring request, then the parameters are stored in the following class
    /// </summary>
    public class sendTutorRequestParam
    {
        public string? CourseName { get; set; }
        public int[]? Days { get; set; }
        public int? tutorId { get; set; }
        public int? studId { get; set; }
        public string? Message { get; set; }
    }

    /// <summary>
    /// When the tutoring requests are retreived by the tutor, then the following class holds the returning list
    /// of requests
    /// </summary>
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
        public string? Message { get; set; }
    }

    /// <summary>
    /// When the tutor accepts the tutoring requests, then the accepted requests are returned to tutor side in the following class
    /// format
    /// </summary>
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
        public string? Message { get; set; }
    }

    /// <summary>
    /// When tutor sends the time slots, then the slots are contained in the following class as parameters
    /// </summary>
    public class SendAppointSlots
    {
        public int? requestId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public string? Slot3 { get; set; }
        public string? Slot4 { get; set; }
        public string? Slot5 { get; set; }
        public string? Message { get; set; }
    }

    /// <summary>
    /// After student selects one tiem slot out of the options, then the appointment becomes confirmed.
    /// The following class contains the return data when the data is sent to tutor side
    /// </summary>
    public class GetConfirmedAppoint
    {
        public string? Name { get; set; }
        public string? Course { get; set; }
        public string? Slot { get; set; }
        public int? Id { get; set; }
    }

    /// <summary>
    /// This class is a parameter holding class that is passed to the method which adds the confirmed appointment to upcoming appointments
    /// 
    /// </summary>
    public class AddToAppoints
    {
        public int? slotId { get; set; }
        public string? paypal { get; set; }
        public string? zoom { get; set; }
    }

    /// <summary>
    /// This class is shared by student and tutor side and it is used to contain return data for when the return
    /// data is sent to display upcoming appointments
    /// </summary>
    public class GetAppointments
    {
        public int? ConfirmId { get; set; }
        public string? Date { get; set; }
        public string? Course { get; set; }
        public string? TutorStud { get; set; }
        public string? Paypal { get; set; }
        public string? Zoom { get; set; }
    }

    /// <summary>
    /// This class is used a parameter holding class in asking query and sending reply methods
    /// It is also used as a return class in get queries method
    /// </summary>
    public class AskQuery
    {
        public string? UserId { get; set; }
        public string? Query { get; set; }
        public int? QueryId { get; set; }
    }

    /// <summary>
    /// This class holds parameters while passing it to Report Account method in studnet and tutor side
    /// 
    /// </summary>
    public class ReportAccountRequest
    {
        public int? UserId { get; set; }
        public int? AccountId { get; set; }
    }

    /// <summary>
    /// This class is parameter class for Delete user method of admin side and it also acts as a return class for
    /// getting reported accounts in admin side
    /// </summary>
    public class GetReportAccount
    {
        public string? Name { get; set; }
        public string? By { get; set; }
        public int? AccountId { get; set; }
    }

    /// <summary>
    /// This class is a return class which is used by admin side to get the statistics
    /// </summary>
    public class GetStats
    {
        public string? Name { get; set; }
        public string? Data { get; set; }
    }

    /// <summary>
    /// The class is a return class that holds the replies for student and tutor side
    /// </summary>
    public class GetReplies
    {
        public string? question { get; set; }
        public string? answer { get; set; }
    }

}
