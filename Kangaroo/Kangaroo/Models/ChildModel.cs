using Kangaroo.Helpers;
using Kangaroo.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class ChildModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _child_face;
        #endregion

        #region Properties
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
        public string daycare_id { get; set; }
        public string class_id { get; set; }
        public string teacher_id { get; set; }
        public string teacher_name { get; set; }
        public string teacher_mobile { get; set; }
        public string parent_id { get; set; }
        public string parent_name { get; set; }
        public string parent_mobile { get; set; }

        public string date_of_birth { get; set; }
        public string gender { get; set; }
        public string gender_name
        {
            get
            {
                if (gender == "1") return AppResources.lbMale;
                else if (gender == "2") return AppResources.lbFemale;
                else return "";
            }
        }
        public string siblings_number { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class ChildResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public ChildModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class ChildListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<ChildModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
