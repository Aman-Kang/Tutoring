using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class ReportAccount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }

        public virtual User Account { get; set; } = null!;
        public virtual StudTutorInfo User { get; set; } = null!;
    }
}
