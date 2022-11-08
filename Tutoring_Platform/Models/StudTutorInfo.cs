using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class StudTutorInfo
    {
        public StudTutorInfo()
        {
            AppointRequests = new HashSet<AppointRequest>();
            HelpQueries = new HashSet<HelpQuery>();
            ReportAccounts = new HashSet<ReportAccount>();
            TutorInfos = new HashSet<TutorInfo>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string? School { get; set; }
        public string? StudyField { get; set; }
        public string? Program { get; set; }
        public int? Semester { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<AppointRequest> AppointRequests { get; set; }
        public virtual ICollection<HelpQuery> HelpQueries { get; set; }
        public virtual ICollection<ReportAccount> ReportAccounts { get; set; }
        public virtual ICollection<TutorInfo> TutorInfos { get; set; }
    }
}
