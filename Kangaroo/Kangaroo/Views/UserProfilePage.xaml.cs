using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
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
	public partial class UserProfilePage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public UserProfilePage()
        {
            InitializeComponent();

            var vm = new UserViewModel(Navigation);
            vm.OnLoadUserData();
            BindingContext = vm;
        }
        #endregion

        #region Events
        private async void ChangePassword_Tapped(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ChangePasswordPage());
            isTapped = false;
        }
        #endregion

    }
}