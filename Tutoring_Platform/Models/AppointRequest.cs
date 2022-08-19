using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
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

        public virtual StudTutorInfo Stud { get; set; } = null!;
        public virtual TutorInfo Tutor { get; set; } = null!;
        public virtual ICollection<AppointSlot> AppointSlots { get; set; }
    }
}
