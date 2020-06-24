using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class HolidayModel
    {
        public string holiday_type { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
        public string time_from { get; set; }
        public string time_to { get; set; }
    }
}
