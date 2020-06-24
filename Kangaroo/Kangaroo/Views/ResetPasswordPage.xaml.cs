using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResetPasswordPage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public ResetPasswordPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private async void cmdResetPassword_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            if (string.IsNullOrEmpty(txtMobile.Text))
            {
                await Utility.ShowNotification("", AppResources.msgReqMobile);
                return;
            }

            var vm = new LoginViewModel(Navigation);
            await vm.OnResetPassword(txtMobile.Text, txtCode.Text, txtNewPassword.Text);
            isTapped = false;
        }
        #endregion

    }
}