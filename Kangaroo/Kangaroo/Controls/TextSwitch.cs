using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TextSwitch : Grid
    {
        private static readonly Color lightGray = Color.FromHex("E0E0E0");
        private static readonly Color darkGray = Color.FromHex("C5C5C5");

        public static readonly BindableProperty OnTextProperty = BindableProperty.Create(nameof(OnText), typeof(string), typeof(TextSwitch), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay, propertyChanged: HandleOnTextPropertyChanged);
        private static void HandleOnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        public static readonly BindableProperty OffTextProperty = BindableProperty.Create(nameof(OffText), typeof(string), typeof(TextSwitch), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay, propertyChanged: HandleOffTextPropertyChanged);
        private static void HandleOffTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TextSwitch), defaultValue: Color.White, propertyChanged: HandleTextColorPropertyChanged);
        private static void HandleTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        public static readonly BindableProperty OnColorProperty = BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(TextSwitch), defaultValue: Color.Default, propertyChanged: HandleOnColorPropertyChanged);
        private static void HandleOnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        public static readonly BindableProperty OffColorProperty = BindableProperty.Create(nameof(OffColor), typeof(Color), typeof(TextSwitch), defaultValue: Color.Default, propertyChanged: HandleOffColorPropertyChanged);
        private static void HandleOffColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        public static readonly BindableProperty IsOnProperty = BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(TextSwitch), defaultValue: false, propertyChanged: HandleIsOnPropertyChanged, defaultBindingMode: BindingMode.TwoWay);

        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }

        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color OnColor
        {
            get { return (Color)GetValue(OnColorProperty); }
            set { SetValue(OnColorProperty, value); }
        }

        public Color OffColor
        {
            get { return (Color)GetValue(OffColorProperty); }
            set { SetValue(OffColorProperty, value); }
        }

        private static void HandleIsOnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as TextSwitch)?.Rebuild();
        }

        FlowDirection GetParentFlow(VisualElement element)
        {
            if (element.FlowDirection == FlowDirection.MatchParent)
            {
                if (element.Parent is VisualElement visualParent)
                    return GetParentFlow(visualParent);
                else
                    return element.FlowDirection;
            }
            else
            {
                return element.FlowDirection;
            }
        }

        private void Rebuild()
        {
            if (IsOn) // make it On
            {
                frmContainer.TranslateTo(70, 0);
                frmContainer.BackgroundColor = lbText.BackgroundColor = OnColor;
                lbText.Text = OnText;
            }
            else // make it Off
            {
                frmContainer.TranslateTo(0, 0);
                frmContainer.BackgroundColor = lbText.BackgroundColor = OffColor;
                lbText.Text = OffText;
            }
        }

        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        Frame frmContainer;
        Label lbText;

        public TextSwitch()
        {
            InitGrid();
            InitFrame();
            InitLabel();
            frmContainer.Content = lbText;
            Children.Add(frmContainer);
            Rebuild();
        }

        private void InitFrame()
        {
            frmContainer = new Frame
            {
                Padding = new Thickness(10, 1),
                BackgroundColor = darkGray,
                HorizontalOptions = LayoutOptions.Start
            };
        }

        private void InitLabel()
        {
            lbText = new Label
            {
                Text = OffText,
                BackgroundColor = darkGray,
                TextColor = TextColor,
                HorizontalOptions = LayoutOptions.Center
            };
        }

        private void SwitchChanged(object sender, EventArgs e)
        {
            Rebuild();
            IsOn = !IsOn;
        }

        private void InitGrid()
        {
            WidthRequest = 120;
            BackgroundColor = lightGray;
            HorizontalOptions = LayoutOptions.Center;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += SwitchChanged;
            GestureRecognizers.Add(tap);
            FlowDirection = FlowDirection.LeftToRight;
        }
    }
}