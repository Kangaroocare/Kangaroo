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
	public partial class ActivationPage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        private int nUserId = 0;
        private string sMobileNo = "";
        #endregion

        #region Functions
        public ActivationPage(int userId, string mobileNo)
        {
            InitializeComponent();
            nUserId = userId;
            sMobileNo = mobileNo;
            EnableResend();
        }

        private async void EnableResend()
        {
            while (true)
            {
                await Task.Delay(60000);
                cmdResend.IsEnabled = true;
                cmdResend.BackgroundColor = (Color)Application.Current.Resources["BrandColor"];
            }
        }
        #endregion

        #region Events
        private async void CmdActivate_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            if (string.IsNullOrEmpty(txtActivationCode.Text))
            {
                await Utility.ShowNotification("", AppResources.msgReqActivationCode);
                return;
            }

            var vm = new UserViewModel(Navigation);
            await vm.OnActivate(nUserId, sMobileNo, txtActivationCode.Text);
            isTapped = false;
        }

        private async void CmdResend_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;
            isTapped = true;

            var vm = new UserViewModel(Navigation);
            await vm.OnResend(nUserId);
            cmdResend.IsEnabled = false;
            cmdResend.BackgroundColor = Color.FromHex("#e5e5e5");
            EnableResend();
            isTapped = false;
        }
        #endregion

    }
}