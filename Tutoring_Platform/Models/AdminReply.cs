using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class AdminReply
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;

        public virtual User Admin { get; set; } = null!;
        public virtual StudTutorInfo User { get; set; } = null!;
    }
}
