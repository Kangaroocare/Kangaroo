using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using MediaManager;
using Plugin.Media.Abstractions;
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
    public partial class VideoPlayerPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public VideoPlayerPage(string videoUrl)
        {
            InitializeComponent();

            CrossMediaManager.Current.Play(videoUrl);
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            
        }
        #endregion

    }
}