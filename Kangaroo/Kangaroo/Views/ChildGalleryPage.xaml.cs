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
	public partial class ChildGalleryPage : BasePage
	{

        #region Declarations
        
        #endregion

        #region Functions
        public ChildGalleryPage(string childId, string childName)
        {
            InitializeComponent();

            barTitle.Title = childName;
            var vm = new ChildViewModel(Navigation);
            vm.OnGetChildGallery(childId);
            this.BindingContext = vm;
        }
        #endregion

        #region Events

        #endregion

    }
}