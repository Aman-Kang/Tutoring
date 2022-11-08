using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
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
