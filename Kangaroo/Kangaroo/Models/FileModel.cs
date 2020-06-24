using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Kangaroo.Models
{
    public class FileModel : INotifyPropertyChanged
    {

        #region Declarations
        private string _file;
        #endregion

        #region Properties
        public int serial { get; set; }

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
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion
        
    }
}
