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
	public partial class DayCareDetailsPage : BasePage
	{

        #region Declarations
        
        #endregion

        #region Functions
        public DayCareDetailsPage(string dayCareId)
        {
            InitializeComponent();

            var vm = new DayCareViewModel(Navigation);
            vm.OnGetDayCareDetails(dayCareId);
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        
        #endregion

    }
}