using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DynamicPopup : PopupPage
    {
        private bool v;
        private object p1;
        private object p2;

        public DynamicPopup(bool showCancellButton, params (string btnName, Action btnAction)[] btns)
        {
            InitializeComponent();
            stkCancel.IsVisible = showCancellButton;
            BoxView hBox = new BoxView
            {
                Style = (Style)Application.Current.Resources["hSeparator"],
                HeightRequest = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            int i = 0;
            foreach (var btn in btns)
            {
                i++;
                var stack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 10)
                };

                stack.Children.Add(new CustomLabel
                {
                    Style = (Style)Application.Current.Resources["LabelStyle"],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Text = btn.btnName,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                });
                stack.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        btn.btnAction();
                        CloseAllPopup();
                    })
                });

                stkParent.Children.Add(stack);
                if (btns.Count() > i)
                {
                    hBox = new BoxView
                    {
                        Style = (Style)Application.Current.Resources["hSeparator"],
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    stkParent.Children.Add(hBox);
                }
            }
        }

        public DynamicPopup(bool showCancellButton, params (string btnName, Action btnAction, Color textColor)[] btns)
        {
            InitializeComponent();
            stkCancel.IsVisible = showCancellButton;
            BoxView hBox = new BoxView
            {
                Style = (Style)Application.Current.Resources["hSeparator"],
                HeightRequest = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            
            int i = 0;
            foreach (var btn in btns)
            {
                i++;
                var stack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(0, 0, 0, 10)
                };

                stack.Children.Add(new CustomLabel
                {
                    Style = (Style)Application.Current.Resources["LabelStyle"],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Text = btn.btnName,
                    TextColor = btn.textColor,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                });
                stack.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        btn.btnAction();
                        CloseAllPopup();
                    })
                });

                stkParent.Children.Add(stack);
                if (btns.Count() > i)
                {
                    hBox = new BoxView
                    {
                        Style = (Style)Application.Current.Resources["hSeparator"],
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    stkParent.Children.Add(hBox);
                }
            }
        }

        public DynamicPopup(bool v, object p1, object p2)
        {
            this.v = v;
            this.p1 = p1;
            this.p2 = p2;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            CloseAllPopup();
            return true;
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
    }
}