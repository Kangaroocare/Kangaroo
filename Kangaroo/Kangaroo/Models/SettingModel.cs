using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class SettingModel
    {
        public string show_dollar { get; set; }
        public string show_rate { get; set; }
        public string dollar_rate { get; set; }
        public string customer_support { get; set; }
        public CountryModel default_country { get; set; }
    }

    public class SettingResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public SettingModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
