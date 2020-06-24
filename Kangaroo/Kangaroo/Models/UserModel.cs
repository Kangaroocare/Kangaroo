using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Kangaroo.Models
{
    public class UserModel
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string city_id { get; set; }
        public string city_name { get; set; }
        public string country_id { get; set; }
        public string country_key { get; set; }
        public string country_name { get; set; }
        public string call_key { get; set; }
        public string code { get; set; }
        public string flag { get; set; }
        public string email { get; set; }
        public string activation_code { get; set; }
        public string mobile { get; set; }
        public string national_id { get; set; }
        public string gender { get; set; }
        public string user_type { get; set; }
        public string qualification { get; set; }
        public string working_hours_from { get; set; }
        public string working_hours_to { get; set; }
        public string child_relation_id { get; set; }
        public string job_role_id { get; set; }
        public string address { get; set; }
        public string mobile_activated { get; set; }
        public string active { get; set; }

        public string password { get; set; }
        public string confirm_password { get; set; }
        public string confirm_email { get; set; }
    }

    public class UserResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public UserModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
