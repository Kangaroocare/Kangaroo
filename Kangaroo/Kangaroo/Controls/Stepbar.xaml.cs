using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.ViewModels;
using Kangaroo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stepbar : ContentView
    {
        #region Declarations
        private int _CurrentStep;
        private Color _FirstStepColor;
        private Color _SecondStepColor;
        private Color _ThirdStepColor;
        private Color _FourthStepColor;

        public static readonly BindableProperty CurrentStepProperty = BindableProperty.Create("CurrentStep", typeof(int), typeof(ContentView), 0, propertyChanged: OnCurrentStepChanged);
        #endregion

        #region Properties
        public int CurrentStep
        {
            get { return _CurrentStep; }
            set
            {
                _CurrentStep = value;
                OnPropertyChanged();
            }
        }

        public Color FirstStepColor
        {
            get { return _FirstStepColor; }
            set
            {
                _FirstStepColor = value;
                OnPropertyChanged();
            }
        }

        public Color SecondStepColor
        {
            get { return _SecondStepColor; }
            set
            {
                _SecondStepColor = value;
                OnPropertyChanged();
            }
        }

        public Color ThirdStepColor
        {
            get { return _ThirdStepColor; }
            set
            {
                _ThirdStepColor = value;
                OnPropertyChanged();
            }
        }

        public Color FourthStepColor
        {
            get { return _FourthStepColor; }
            set
            {
                _FourthStepColor = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Functions
        public Stepbar()
        {
            InitializeComponent();
            
            if (Settings.Language == "en") FlowDirection = FlowDirection.LeftToRight;
            else FlowDirection = FlowDirection.RightToLeft;

            CurrentStep = 1;
            FirstStepColor = (Color)Application.Current.Resources["GrayLight"];
            SecondStepColor = (Color)Application.Current.Resources["GrayLight"];
            ThirdStepColor = (Color)Application.Current.Resources["GrayLight"];
            FourthStepColor = (Color)Application.Current.Resources["GrayLight"];
        }
        #endregion

        #region Events
        private static void OnCurrentStepChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;

            var stepbar = bindable as Stepbar;
            switch (Utility.GetInteger(newValue))
            {
                case 1:
                    stepbar.FirstStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.SecondStepColor = (Color)Application.Current.Resources["GrayLight"];
                    stepbar.ThirdStepColor = (Color)Application.Current.Resources["GrayLight"];
                    stepbar.FourthStepColor = (Color)Application.Current.Resources["GrayLight"];
                    break;
                case 2:
                    stepbar.FirstStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.SecondStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.ThirdStepColor = (Color)Application.Current.Resources["GrayLight"];
                    stepbar.FourthStepColor = (Color)Application.Current.Resources["GrayLight"];
                    break;
                case 3:
                    stepbar.FirstStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.SecondStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.ThirdStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.FourthStepColor = (Color)Application.Current.Resources["GrayLight"];
                    break;
                case 4:
                    stepbar.FirstStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.SecondStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.ThirdStepColor = (Color)Application.Current.Resources["BrandColor"];
                    stepbar.FourthStepColor = (Color)Application.Current.Resources["BrandColor"];
                    break;
            }
        }
        #endregion

    }
}