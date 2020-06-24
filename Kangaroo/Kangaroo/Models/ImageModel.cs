using Kangaroo.Helpers;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Kangaroo.Models
{
    public class ImageModel : INotifyPropertyChanged
    {

        #region Declarations
        public int serial { get; set; }

        public string media_type { get; set; }

        public string file_path { get; set; }

        public string extension { get; set; }

        public string image_content { get; set; }

        public Stream video_stream { get; set; }

        public string full_path { get; set; }
        #endregion

        #region Functions
        public ImageModel()
        {
            media_type = "Photo";
        }

        public string GetExtension()
        {
            if (file_path == null) return "jpg";
            int nExtensionIndex = file_path.LastIndexOf('.') + 1;
            return file_path.Substring(nExtensionIndex, (file_path.Length - nExtensionIndex));
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

    }
}
