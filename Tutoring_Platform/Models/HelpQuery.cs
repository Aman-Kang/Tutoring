using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class HelpQuery
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; } = null!;

        public virtual StudTutorInfo User { get; set; } = null!;
    }
}
