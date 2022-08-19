using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class DaysAvailable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        public virtual TutorInfo User { get; set; } = null!;
    }
}
