using Kangaroo.ViewModels;
using Kangaroo.Helpers;
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
    public partial class DayCaresView : ContentView
    {

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public DayCaresView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void txtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void txtSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            await Utility.ShowNotification("", "You click search button");
        }
        #endregion

    }
}