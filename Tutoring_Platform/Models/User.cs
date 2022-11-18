using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// User class holds the users high level data in the database
    /// </summary>
    public partial class User
    {
        public User()
        {
            AdminReplies = new HashSet<AdminReply>();
            ReportAccounts = new HashSet<ReportAccount>();
            StudTutorInfos = new HashSet<StudTutorInfo>();
        }

        public int Id { get; set; }
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<AdminReply> AdminReplies { get; set; }
        public virtual ICollection<ReportAccount> ReportAccounts { get; set; }
        public virtual ICollection<StudTutorInfo> StudTutorInfos { get; set; }
    }
}
