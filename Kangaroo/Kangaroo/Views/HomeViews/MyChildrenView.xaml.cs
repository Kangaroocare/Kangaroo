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
    public partial class MyChildrenView : ContentView
    {

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public MyChildrenView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private async void cmdAddChild_Clicked(object sender, EventArgs e)
        {
            if (isTapped) return;

            isTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new AddChildPage());
            isTapped = false;
        }
        #endregion

    }
}