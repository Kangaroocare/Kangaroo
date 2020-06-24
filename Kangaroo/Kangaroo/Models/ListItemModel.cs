using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kangaroo.Models
{
    public class ListItemModel : INotifyPropertyChanged
    {
        public string id { get; set; }
        public string name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
