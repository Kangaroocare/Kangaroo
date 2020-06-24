using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class ClassModel
    {
        public string id { get; set; }
        public string class_name { get; set; }
        public string class_grade { get; set; }
        public string related_kids { get; set; }
        public string attended_kids { get; set; }
        public string absent_kids { get; set; }

        public string class_color { get; set; }
    }

    public class ClassResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public ClassModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class ClassListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<ClassModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
