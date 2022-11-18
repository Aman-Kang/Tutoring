using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// HelpQuery holds the data related to help queries submitted by users
    /// </summary>
    public partial class HelpQuery
    {
        public HelpQuery()
        {
            AdminReplies = new HashSet<AdminReply>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; } = null!;

        public virtual StudTutorInfo User { get; set; } = null!;
        public virtual ICollection<AdminReply> AdminReplies { get; set; }
    }
}
