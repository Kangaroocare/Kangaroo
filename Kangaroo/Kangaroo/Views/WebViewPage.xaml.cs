using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage : BasePage
    {

        #region Declarations
        private string _ShareUrl = "";
        private string _WebViewSource = "";
        private SourceType _SourceType = SourceType.URL;
        #endregion

        #region Enums
        public enum SourceType
        {
            URL = 1,
            HTML = 2
        }
        #endregion

        #region Functions
        public WebViewPage(string webViewSource, SourceType pageSourceType, string pageTitle, string shareUrl)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                barTitle.Title = pageTitle;
                _WebViewSource = webViewSource;
                _SourceType = pageSourceType;
                _ShareUrl = shareUrl;
                Title = pageTitle;
                LoadWebView();
            }
            catch (Exception ex)
            {
                Log.Report(ex);
            }
        }

        private async void LoadWebView()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Utility.ShowNotification("", AppResources.msgNoInternetConnection);
                return;
            }

            switch (_SourceType)
            {
                case SourceType.HTML:
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = _WebViewSource;
                    webView.Source = htmlSource;
                    break;
                case SourceType.URL:
                    webView.Source = _WebViewSource;
                    break;
            }
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {

        }
        #endregion

    }
}