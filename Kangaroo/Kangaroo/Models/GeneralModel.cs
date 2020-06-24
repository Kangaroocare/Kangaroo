using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class GeneralModel : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public object data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
