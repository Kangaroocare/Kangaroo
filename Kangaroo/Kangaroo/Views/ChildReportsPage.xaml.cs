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
    public partial class ChildReportsPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public ChildReportsPage(string childId, string childName)
        {
            InitializeComponent();

            barTitle.Title = string.Format("{0} {1}", (Settings.Language == "ar" ? AppResources.lbReports : childName), (Settings.Language == "ar" ? childName : AppResources.lbReports));
            var vm = new DailyReportViewModel(Navigation);
            vm.OnGetChildDailyReports(childId);
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