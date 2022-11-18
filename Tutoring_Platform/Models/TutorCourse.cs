using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// TutorCourse class contains the data of the courses that a tutor wants to teach. A tutor can have a
    /// maximum of three courses at a time in the database.
    /// </summary>
    public partial class TutorCourse
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public string Course { get; set; } = null!;

        public virtual TutorInfo Tutor { get; set; } = null!;
    }
}
