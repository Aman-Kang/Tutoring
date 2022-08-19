using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class TutorCourse
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public string Course { get; set; } = null!;

        public virtual TutorInfo Tutor { get; set; } = null!;
    }
}
