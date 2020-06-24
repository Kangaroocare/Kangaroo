using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kangaroo.Helpers;
using Kangaroo.ViewModels;

namespace Kangaroo.Models
{
    public class ClassChildrenModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _child_face;
        private string _is_attend;
        #endregion

        #region Properties
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

        public string is_attend
        {
            get { return _is_attend; }
            set
            {
                _is_attend = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(child_face)));

                if (!string.IsNullOrEmpty(class_id) && !string.IsNullOrEmpty(child_id))
                {
                    var vm = new ClassViewModel();
                    vm.OnRecordAttendance(class_id, child_id, (value == "1" ? true : false));
                }
            }
        }

        public string parent_id { get; set; }

        public string parent_name { get; set; }

        public string parent_mobile { get; set; }

        public string daycare_id { get; set; }

        public string daycare_name { get; set; }

        public string class_id { get; set; }

        public string child_id { get; set; }

        public string teacher_id { get; set; }

        public string teacher_name { get; set; }

        public string teacher_mobile { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class ClassChildrenResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<ClassChildrenModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
