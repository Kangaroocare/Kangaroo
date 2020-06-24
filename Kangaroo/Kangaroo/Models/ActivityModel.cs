using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace Kangaroo.Models
{
    public class ActivityModel : INotifyPropertyChanged
    {

        #region Declarations

        #endregion

        #region Properties
        public string id { get; set; }
        public string daycare_id { get; set; }
        public string activity_time { get; set; }
        public string activity_desc { get; set; }
        public string activity_type_id { get; set; }
        public string nutrition_type_id { get; set; }
        public int order { get; set; } // Used when add report
        public int sort { get; set; } // Used when read report

        public int serial { get; set; }
        public string activity_type_name { get; set; }
        public string nutrition_type_name { get; set; }

        public bool is_default_header { get; set; }
        public bool is_default_item { get; set; }
        public bool is_nutrition_header { get; set; }
        public bool is_nutrition_item { get; set; }
        public bool is_supply_header { get; set; }
        public bool is_supply_item { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
    }

    public class ActivityResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public ActivityModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class ActivityListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<ActivityModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
