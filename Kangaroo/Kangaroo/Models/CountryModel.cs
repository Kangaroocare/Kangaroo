using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class CountryModel
    {
        public string country_id { get; set; }
        public string country_name { get; set; }
        public string country_key { get; set; }
        public string call_key { get; set; }
        public string mobile_ex { get; set; }
        public string flag { get; set; }
        public string flag_path { get; set; }
    }

    public class CountryResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public CountryModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class CountryListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<CountryModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
