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
    public partial class NotificationPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public NotificationPage(string parentId)
        {
            InitializeComponent();

            barTitle.Title = AppResources.lbParentNotifications;
            var vm = new NotificationViewModel(Navigation);
            vm.OnGetParentNotifications(parentId);
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        private void BasePage_Appearing(object sender, EventArgs e)
        {
            
        }
        #endregion

    }
}