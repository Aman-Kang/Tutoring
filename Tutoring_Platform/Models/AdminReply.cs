using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// AdminReply class is a data class that stores the replies from admin
    /// </summary>
    public partial class AdminReply
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int QueryId { get; set; }
        public string Message { get; set; } = null!;

        public virtual User Admin { get; set; } = null!;
        public virtual HelpQuery Query { get; set; } = null!;
    }
}
