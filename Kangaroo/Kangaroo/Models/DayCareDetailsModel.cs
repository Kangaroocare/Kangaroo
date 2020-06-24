using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kangaroo.Helpers;
using Kangaroo.Resources;

namespace Kangaroo.Models
{
    public class DayCareDetailsModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _photo;
        private string _favorite_color = "#703081";

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Properties
        public string daycare_name { get; set; }

        public string description { get; set; }

        public string school_admin_id { get; set; }

        public string contact_number { get; set; }

        public string website_url { get; set; }

        public string working_hours { get; set; }

        public string working_days { get; set; }

        public string init_date { get; set; }

        public string is_special_needs { get; set; }

        public string is_special_needs_text
        {
            get { return (is_special_needs == "1" ? AppResources.lbYes : AppResources.lbNo); }
        }

        public string is_favorite { get; set; }

        public List<HolidayModel> holidays { get; set; }

        public string photo
        {
            get { return (!string.IsNullOrEmpty(_photo) ? (Utility.ServerUrl + _photo) : ""); }
            set
            {
                _photo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(photo)));
            }
        }

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

    }

    public class DayCareDetailsResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public DayCareDetailsModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
