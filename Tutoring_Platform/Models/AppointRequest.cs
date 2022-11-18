using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// AppointRequest is a data class that stores the appointment requests made by the students.
    /// </summary>
    public partial class AppointRequest
    {
        public AppointRequest()
        {
            AppointSlots = new HashSet<AppointSlot>();
        }

        public int Id { get; set; }
        public int StudId { get; set; }
        public int TutorId { get; set; }
        public string Course { get; set; } = null!;
        public int? Sunday { get; set; }
        public int? Monday { get; set; }
        public int? Tuesday { get; set; }
        public int? Wednesday { get; set; }
        public int? Thursday { get; set; }
        public int? Friday { get; set; }
        public int? Saturday { get; set; }
        public string? Message { get; set; }

        public virtual StudTutorInfo Stud { get; set; } = null!;
        public virtual TutorInfo Tutor { get; set; } = null!;
        public virtual ICollection<AppointSlot> AppointSlots { get; set; }
    }
}
