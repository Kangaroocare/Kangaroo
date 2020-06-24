using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class ReportModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _child_face;

        private List<FileModel> _report_files;
        private List<ActivityModel> _activities;
        #endregion

        #region Properties
        public string daycare_id { get; set; }

        public string report_day { get; set; }

        public string child_id { get; set; }

        public string child_name { get; set; }

        public string child_face
        {
            get { return (!string.IsNullOrEmpty(_child_face) ? (Utility.ServerUrl + _child_face) : ""); }
            set
            {
                _child_face = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(child_face)));
            }
        }

        public string teacher_id { get; set; }

        public string general_note { get; set; }

        public string parent_note { get; set; }

        public string child_progress { get; set; }

        public string reading_status { get; set; }

        public List<FileModel> report_files
        {
            get { return _report_files; }
            set
            {
                _report_files = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(report_files)));
            }
        }

        public List<ActivityModel> activities
        {
            get { return _activities; }
            set
            {
                _activities = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(activities)));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class ReportResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public ReportModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
