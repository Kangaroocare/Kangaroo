﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Views.ChildViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildView : ContentView
    {
        public ChildView()
        {
            InitializeComponent();
        }
    }
}