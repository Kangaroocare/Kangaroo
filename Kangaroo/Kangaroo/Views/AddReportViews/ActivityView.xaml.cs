﻿using Kangaroo.ViewModels;
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
    public partial class ActivityView : ContentView
    {

        #region Declarations
        private bool isTapped = false;
        #endregion

        #region Functions
        public ActivityView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        
        #endregion

    }
}