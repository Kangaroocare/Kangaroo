using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using Xamarin.Forms;

namespace Kangaroo.Models
{
    public class MessageModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _message;
        #endregion

        #region Properties
        public string id { get; set; }

        public string parent_message_id { get; set; }

        public string sender_id { get; set; }

        public string sender_name { get; set; }

        public string recipient_id { get; set; }

        public string recipient_name { get; set; }

        public string datetime { get; set; }

        public string message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(message)));
            }
        }

        public string seen { get; set; }

        public string background_color { get; set; }

        public string font_color { get; set; }

        public LayoutOptions text_align
        {
            get { return (Settings.UserId == sender_id ? LayoutOptions.Start : LayoutOptions.End); }
        }

        public Thickness message_margin
        {
            get
            {
                if(Settings.Language == "ar") return (Settings.UserId == sender_id ? new Thickness(25, 0, 0, 0) : new Thickness(0, 0, 25, 0));
                else return (Settings.UserId == sender_id ? new Thickness(0, 0, 25, 0) : new Thickness(25, 0, 0, 0));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class MessageResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<MessageModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
