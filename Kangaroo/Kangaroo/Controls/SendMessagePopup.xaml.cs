using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendMessagePopup : PopupPage
    {

        #region Declarations
        private bool isTapped = false;

        private string _SenderId;
        private string _RecipientId;
        #endregion

        #region Functions
        public SendMessagePopup(string senderId, string recipientId)
        {
            InitializeComponent();
            _SenderId = senderId;
            _RecipientId = recipientId;
        }

        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }
        #endregion

        #region Events
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;
        }

        private async void cmdSendMessage_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            //var vm = new ProjectViewModel();
            //await vm.OnSendMessage(_SenderId, _RecipientId, txtMessage.Text);
            isTapped = true;
        }

        protected override bool OnBackButtonPressed()
        {
            CloseAllPopup();
            return true;
        }

        private void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private void cmdClosePopup_Tapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
        #endregion

    }
}