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
	public partial class FavoriteDayCaresPage : BasePage
	{

        #region Declarations
        
        #endregion

        #region Functions
        public FavoriteDayCaresPage()
        {
            InitializeComponent();

            var vm = new DayCareViewModel(Navigation);
            vm.OnGetFavoriteDayCares();
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