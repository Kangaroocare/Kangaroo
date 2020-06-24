using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertPopup : PopupPage
    {

        #region Declarations
        private int interval = 4000;
        #endregion

        #region Functions
        public AlertPopup(string message)
        {
            InitializeComponent();
            lbMessage.Text = message;
            CloseAlert();
        }

        public AlertPopup(string message, int _interval)
        {
            InitializeComponent();
            lbMessage.Text = message;
            interval = _interval;
            CloseAlert();
        }

        private async void CloseAlert()
        {
            await Task.Delay(interval);
            await ClosePopup();
        }

        private async Task ClosePopup()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
                await PopupNavigation.Instance.PopAsync();
        }
        #endregion

        #region Events
        private async void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            await ClosePopup();
        }

        private async void Popup_Tapped(object sender, EventArgs e)
        {
            await ClosePopup();
        }
        #endregion

    }
}