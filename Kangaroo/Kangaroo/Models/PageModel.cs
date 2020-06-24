using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace Kangaroo.Models
{
    public class PageModel : INotifyPropertyChanged
    {

        public string page_id { get; set; }
        public string page_title { get; set; }
        public string page_desc { get; set; }
        public string page_content { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class PageResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public PageModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
