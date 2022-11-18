using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// DaysAvailable class holds the data related to available days of the tutor.
    /// </summary>
    public partial class DaysAvailable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Sunday { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }

        public virtual TutorInfo User { get; set; } = null!;
    }
}
