using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    /// <summary>
    /// AppointSlot is data class that stores the appointment time slots offered by the tutor.
    /// </summary>
    public partial class AppointSlot
    {
        public AppointSlot()
        {
            AppointConfirms = new HashSet<AppointConfirm>();
        }

        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Slot { get; set; } = null!;
        public bool Selected { get; set; }
        public string? Message { get; set; }

        public virtual AppointRequest Request { get; set; } = null!;
        public virtual ICollection<AppointConfirm> AppointConfirms { get; set; }
    }
}
