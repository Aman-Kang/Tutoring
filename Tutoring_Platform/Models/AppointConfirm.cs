using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class AppointConfirm
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public string PaypalLink { get; set; } = null!;
        public string MeetingLink { get; set; } = null!;

        public virtual AppointSlot Slot { get; set; } = null!;
    }
}
