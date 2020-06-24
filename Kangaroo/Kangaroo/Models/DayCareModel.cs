using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kangaroo.Helpers;

namespace Kangaroo.Models
{
    public class DayCareModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _photo;
        private string _is_favorite;
        private string _favorite_color;
        #endregion

        #region Properties
        public string daycare_id { get; set; }

        public string daycare_name { get; set; }

        public string daycare_desc { get; set; }

        public string school_admin_id { get; set; }

        public string lat { get; set; }

        public string lng { get; set; }

        public string distance { get; set; }

        public string photo
        {
            get { return (!string.IsNullOrEmpty(_photo) ? (Utility.ServerUrl + _photo) : ""); }
            set
            {
                _photo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(photo)));
            }
        }

        public string is_favorite
        {
            get { return _is_favorite; }
            set
            {
                _is_favorite = value;
                favorite_color = (_is_favorite == "1" ? "#703081" : "#b2b2b2");
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(is_favorite)));
            }
        }

        public string is_special_needs { get; set; }

        public string city_id { get; set; }

        public string favorite_color
        {
            get { return _favorite_color; }
            set
            {
                _favorite_color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(favorite_color)));
            }
        }

        public bool is_favorite_visible
        {
            get { return (!string.IsNullOrEmpty(Settings.UserId) ? true : false); }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class DayCareResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public DayCareModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class DayCareListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<DayCareModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
