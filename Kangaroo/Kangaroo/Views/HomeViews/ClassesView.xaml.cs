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
    public partial class ClassesView : ContentView
    {

        #region Declarations
        
        #endregion

        #region Functions
        public ClassesView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void txtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Class_Tapped(object sender, EventArgs e)
        {
            
        }
        #endregion

    }
}