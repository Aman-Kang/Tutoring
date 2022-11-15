using System;
using System.Collections.Generic;

namespace Tutoring_Platform.Models
{
    public partial class Statistic
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Data { get; set; } = null!;
    }
}
