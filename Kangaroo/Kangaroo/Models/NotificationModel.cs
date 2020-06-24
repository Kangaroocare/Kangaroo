using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace Kangaroo.Models
{
    public class NotificationModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _image;
        #endregion

        #region Properties
        public string notification_id { get; set; }

        public string sender_id { get; set; }

        public string rec_id { get; set; }

        public string msg_count { get; set; }

        public string sender_name { get; set; }

        public string title { get; set; }

        public string image
        {
            get { return (!string.IsNullOrEmpty(_image) ? (Utility.ServerUrl + _image) : ""); }
            set
            {
                _image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(image)));
            }
        }

        public string font_color { get; set; }

        public string background_color { get; set; }

        public string is_seen { get; set; }

        public string is_pinned { get; set; }

        public string created_at { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class NotificationResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<NotificationModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
