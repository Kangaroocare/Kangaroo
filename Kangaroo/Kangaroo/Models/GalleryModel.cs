using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Kangaroo.Models
{
    public class GalleryModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _file;
        #endregion

        #region Properties
        public string file
        {
            get
            {
                return (!string.IsNullOrEmpty(_file) ? (Utility.ServerUrl + _file) : "");
            }
            set
            {
                _file = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(file)));
            }
        }

        public object image
        {
            get
            {
                if (is_image == "1") return (!string.IsNullOrEmpty(_file.ToString()) ? (Utility.ServerUrl + _file.ToString()) : "");
                else return ImageSource.FromResource("Kangaroo.Images.defaultVid.png");
            }
        }

        public string is_image { get; set; }

        public string uploaded_date { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }

    public class GalleryResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public GalleryModel data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

    public class GalleryListResult : INotifyPropertyChanged
    {
        public string response_status { get; set; }
        public string response_message { get; set; }
        public List<GalleryModel> data { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
