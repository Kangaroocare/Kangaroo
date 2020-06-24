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
	public partial class ForgetPasswordPage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public ForgetPasswordPage(LoginViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        private async void cmdSendResetCode_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            if (string.IsNullOrEmpty(txtMobile.Text))
            {
                await Utility.ShowNotification("", AppResources.msgReqMobile);
                isTapped = false;
                return;
            }

            var vm = this.BindingContext as LoginViewModel;
            await vm.OnForgetPassword(txtMobile.Text);
            isTapped = false;
        }
        #endregion

    }
}