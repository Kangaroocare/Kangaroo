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
    public partial class AddReportPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public AddReportPage(string daycareId, string childId, string childName, string childFace)
        {
            InitializeComponent();

            var vm = new AddReportViewModel(Navigation);
            vm.daycare_id = daycareId;
            vm.report_day = DateTime.Now.ToString("yyyy-MM-dd");
            vm.child_id = childId;
            vm.child_name = childName;
            vm.child_face = childFace;
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