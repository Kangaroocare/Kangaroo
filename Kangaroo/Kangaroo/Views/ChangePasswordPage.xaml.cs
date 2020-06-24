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
	public partial class ChangePasswordPage : BasePage
	{

        #region Declarations
        private bool isTapped;
        #endregion

        #region Functions
        public ChangePasswordPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private async void cmdChangePassword_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            var vm = new UserViewModel(Navigation);
            await vm.OnChangePassword(txtOldPassword.Text, txtNewPassword.Text, txtConfirmPassword.Text);
            isTapped = false;
        }
        #endregion

    }
}