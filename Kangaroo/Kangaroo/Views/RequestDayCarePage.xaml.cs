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
	public partial class RequestDayCarePage : BasePage
	{

        #region Declarations

        #endregion

        #region Functions
        public RequestDayCarePage(string childId, string childName)
        {
            InitializeComponent();

            var vm = new ChildViewModel(Navigation);
            vm.OnGetDayCares();
            vm.ChildData.child_id = childId;
            this.BindingContext = vm;
            txtChildName.Text = childName;
        }
        #endregion

        #region Events

        #endregion

    }
}