﻿using Kangaroo.Controls;
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
    public partial class AddGalleryPage : BasePage
    {

        #region Declarations

        #endregion

        #region Functions
        public AddGalleryPage(string childId)
        {
            InitializeComponent();

            barTitle.Title = AppResources.lbUploadGallery;
            var vm = new AddGalleryViewModel(Navigation);
            vm.child_id = childId;
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