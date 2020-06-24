using Kangaroo.Helpers;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPopup : PopupPage
    {
        public DialogPopup(string message, (string okText, Action okAction) okBtn, string cancellText)
        {
            this.FlowDirection = Settings.FlowDirection;
            InitializeComponent();
            lbMessage.Text = message;
            btnOk.Text = okBtn.okText;
            btnCancel.Text = cancellText;

            btnOk.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { okBtn.okAction(); CloseAllPopup(); }) });
            btnCancel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { CloseAllPopup(); CloseAllPopup(); }) });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;
        }

        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            CloseAllPopup();
            return true;
        }
    }
}