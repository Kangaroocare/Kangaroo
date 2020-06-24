using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using MediaManager;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageViewerPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public ImageViewerPage(string imageUrl)
        {
            InitializeComponent();

            imgFile.Source = ImageSource.FromUri(new Uri(imageUrl));
        }

        public ImageViewerPage(ImageSource imageSource)
        {
            InitializeComponent();

            imgFile.Source = imageSource;
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            
        }
        #endregion

    }
}