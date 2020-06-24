using Kangaroo.Helpers;
using Kangaroo.Resources;
using Kangaroo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Toolbar : ContentView
    {
        #region Declarations
        public static readonly BindableProperty IsMainProperty = BindableProperty.Create("IsMain", typeof(bool), typeof(ContentView), false, propertyChanged: OnIsMainChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(ContentView), "", propertyChanged: OnTitleChanged);
        #endregion

        #region Properties
        public string Title
        {
            get { return (string)base.GetValue(TitleProperty); }
            set
            {
                base.SetValue(TitleProperty, value);
            }
        }
        #endregion

        #region Properties
        public bool IsMain
        {
            get { return (bool)GetValue(IsMainProperty); }
            set { SetValue(IsMainProperty, value); }
        }
        #endregion

        #region Functions
        public Toolbar()
        {
            InitializeComponent();

            if (Settings.Language == "en")
            {
                FlowDirection = FlowDirection.LeftToRight;
                btnBack.RotationY = 180;
            }
            else FlowDirection = FlowDirection.RightToLeft;
        }
        #endregion

        #region Events
        private void Menu_Tapped(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.LastOrDefault()?.GetType().Name == "HomePage")
                (Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as MasterDetailPage).IsPresented = true;
        }

        public async virtual void Back_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private static void OnIsMainChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;

            var toolbar = bindable as Toolbar;
            toolbar.slMenu.IsVisible = Convert.ToBoolean(newValue);
            toolbar.slBack.IsVisible = !Convert.ToBoolean(newValue);
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var toolbar = bindable as Toolbar;
            toolbar.lbTitle.Text = newValue == null ? string.Empty : newValue.ToString();
        }
        #endregion

    }
}