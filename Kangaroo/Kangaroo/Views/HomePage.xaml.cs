using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using Rg.Plugins.Popup.Extensions;
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
    public partial class HomePage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public HomePage()
        {
            InitializeComponent();

            var vm = new HomeViewModel(Navigation);
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            var vm = this.BindingContext as HomeViewModel;
            switch (Settings.HomeTabIndex)
            {
                case 0:
                    vm.OnFirstView();
                    break;
                case 1:
                    vm.OnSecondView();
                    break;
                case 2:
                    vm.OnThirdView();
                    break;
                case 3:
                    vm.OnFourthView();
                    break;
            }
        }
        #endregion

    }
}