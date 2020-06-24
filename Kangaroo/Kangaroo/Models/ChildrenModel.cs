using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class ChildrenModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _child_face;
        #endregion

        #region Properties
        public string id { get; set; }
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
        public string is_attend { get; set; }
        public string mobile { get; set; }

        public string national_id { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string mother_name { get; set; }
        public string mother_national_id { get; set; }
        public string mother_address { get; set; }
        public string mother_mobile { get; set; }
        public string father_name { get; set; }
        public string father_national_id { get; set; }
        public string father_address { get; set; }
        public string father_mobile { get; set; }
        public string siblings_number { get; set; }
        public string health_condition { get; set; }
        public string what_to_know { get; set; }
        public string em_contact_name { get; set; }
        public string em_contact_mobile { get; set; }
        public string start_date { get; set; }
        //public string end_date { get; set; }
        #endregion

        #region Functions
        public ChildrenModel()
        {
            date_of_birth = DateTime.Now.ToString("yyyy-MM-dd");
            start_date = DateTime.Now.ToString("yyyy-MM-dd");
            //end_date = DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class ChildrenResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public ChildrenModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class ChildrenListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<ChildrenModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
