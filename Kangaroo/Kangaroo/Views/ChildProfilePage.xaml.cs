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
	public partial class ChildProfilePage : BasePage
	{

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public ChildProfilePage(string childId)
        {
            InitializeComponent();

            barTitle.Title = AppResources.lbChildProfile;
            var vm = new ChildViewModel(Navigation);
            vm.OnGetChildById(childId);
            this.BindingContext = vm;
        }
        #endregion

        #region Events
        
        #endregion

    }
}