using Kangaroo.Controls;
using Kangaroo.Helpers;
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
	public partial class LoginPage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel(Navigation);
        }
        #endregion

        #region Events
        private async void ForgetPassword_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            var vm = this.BindingContext as LoginViewModel;
            await Application.Current.MainPage.Navigation.PushAsync(new ForgetPasswordPage(vm));
            isTapped = false;
        }

        private async void CreateAccount_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new CreateUserPage());
            isTapped = false;
        }
        #endregion

    }
}