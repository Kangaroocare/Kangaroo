using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class DailyReportModel
    {

  //      public string id { get; set; }
  //      public string report_day { get; set; }
  //      public string child_id { get; set; }
  //      public string teacher_id { get; set; }
  //      public string bathroom_time { get; set; }
  //      public string bathroom_desc { get; set; }
  //      public string nutrition_time { get; set; }
  //      public string nutrition_category { get; set; }
  //      public string nutrition_desc { get; set; }
  //      public string medication_time { get; set; }
  //      public string medication_dec { get; set; }
  //      public string trip_time { get; set; }
  //      public string trip_desc { get; set; }
  //      public string supply_needs_desc { get; set; }
  //      public string general_note { get; set; }
  //      public string parent_note { get; set; }
  //      public string images { get; set; }
  //      public string child_progress { get; set; }
  //      public string reading_status { get; set; }
  //      public string created_at { get; set; }
  //      public string updated_at { get; set; }
  //      public string child_name { get; set; }
  //      public string child_face { get; set; }
  //      public string mobile { get; set; }

        public string report_id { get; set; }
        public string report_day { get; set; }
        public string daycare_id { get; set; }
        public string child_id { get; set; }
        public string teacher_id { get; set; }
        public string parent_id { get; set; }
        public string general_note { get; set; }
        public string parent_note { get; set; }
        public string child_name { get; set; }
        public string child_progress { get; set; }
        public string reading_status { get; set; }
    }

    public class DailyReportResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public DailyReportModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class DailyReportListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<DailyReportModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
