using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Kangaroo.Resources;

//<Controls:RadioButtonsGroup x:Name="radioGender" VerticalOptions="Start" Grid.Column="1" ItemsSource="{Binding lstGender}" SelectedItem="{Binding SelectedGender}" Orientation="Horizontal" Direction="{Binding Converter={StaticResource langToDirConverter}}" HorizontalOptions="Fill"/>

namespace Kangaroo.Controls
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public object SelectedItem { get; }
        public object SelectedValue { get; }
        public int SelectedIndex { get; }

        public SelectionChangedEventArgs(object selectedItem, object selectedValue, int selectedIndex)
        {
            SelectedItem = selectedItem;
            SelectedIndex = selectedIndex;
            SelectedValue = selectedValue;
        }
    }

    public class ItemsAddedEventArgs : EventArgs
    {
        public IEnumerable<object> Items { get; }
        public ItemsAddedEventArgs(IEnumerable<object> items)
        {
            Items = items;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class RadioButtonsGroup : ContentView
    {
        StackLayout parentStack;
        List<Label> lbRadios;
        readonly string chckd = "◉";
        readonly string unchckd = "◯";

        public RadioButtonsGroup()
        {
            parentStack = new StackLayout();
            Content = parentStack;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(RadioButtonsGroup), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioButtonsGroup), defaultValue: -1, defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonsGroup), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedItemChanged);
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(RadioButtonsGroup), defaultValue: StackOrientation.Vertical);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButtonsGroup), defaultValue: Color.FromHex("#6f6f6f"));
        public static readonly BindableProperty RadioColorProperty = BindableProperty.Create(nameof(RadioColor), typeof(Color), typeof(RadioButtonsGroup), defaultValue: Color.FromHex("#703081"));
        public static readonly BindableProperty DirectionProperty = BindableProperty.Create(nameof(Direction), typeof(FlowDirection), typeof(RadioButtonsGroup), defaultValue: FlowDirection.LeftToRight);
        public static readonly BindableProperty DisplayMemberPathProperty = BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(RadioButtonsGroup));
        public static readonly BindableProperty SelectedValuePathProperty = BindableProperty.Create(nameof(SelectedValuePath), typeof(string), typeof(RadioButtonsGroup));
        public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(nameof(SelectedValue), typeof(object), typeof(RadioButtonsGroup));

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var @this = (RadioButtonsGroup)bindable;
            @this.OnSelectedItemChanged();
        }

        private void OnSelectedItemChanged()
        {
            if (SelectedItem != null)
            {
                try
                {
                    var lstChecked = lbRadios.Where(x => x.Text == chckd).ToList();
                    if (lstChecked != null)
                    {
                        foreach (Label item in lstChecked)
                        {
                            item.Text = unchckd;
                            item.FontSize = item.FontSize - 10;
                        }
                    }
                }
                catch (Exception) { }

                var automationId = SelectedValuePath == null ? SelectedItem.ToString() : SelectedItem.GetType().GetProperty(SelectedValuePath).GetValue(SelectedItem, null).ToString();
                lbRadios.Single(a => a.AutomationId == automationId).FontSize = lbRadios.Single(a => a.AutomationId == automationId).FontSize + 10;
                lbRadios.Single(a => a.AutomationId == automationId).Text = chckd;
            }
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public StackOrientation Orientation
        {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color RadioColor
        {
            get { return (Color)GetValue(RadioColorProperty); }
            set { SetValue(RadioColorProperty, value); }
        }

        public FlowDirection Direction
        {
            get { return (FlowDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public delegate void ItemsAddedHandler(object sender, ItemsAddedEventArgs e);
        public event ItemsAddedHandler OnItemsAdded;

        public delegate void SelectionChangedHandler(object sender, SelectionChangedEventArgs e);
        public event SelectionChangedHandler OnSelectionChanged;

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            var @this = bindable as RadioButtonsGroup;

            // unsubscribe from the old value
            var oldNPC = oldValue as INotifyPropertyChanged;
            if (oldNPC != null)
            {
                oldNPC.PropertyChanged -= @this.OnItemsSourcePropertyChanged;
            }

            var oldNCC = oldValue as INotifyCollectionChanged;
            if (oldNCC != null)
            {
                oldNCC.CollectionChanged -= @this.OnItemsSourceCollectionChanged;
            }

            // subscribe to the new value
            var newNPC = newValue as INotifyPropertyChanged;
            if (newNPC != null)
            {
                newNPC.PropertyChanged += @this.OnItemsSourcePropertyChanged;
            }

            var newNCC = newValue as INotifyCollectionChanged;
            if (newNCC != null)
            {
                newNCC.CollectionChanged += @this.OnItemsSourceCollectionChanged;
            }

            // inform the instance to do something
            @this.RebuildOnItemsSource();
        }

        private void RebuildOnItemsSource()
        {
            if (parentStack.Children.Count > 0) return;

            parentStack.Orientation = Orientation;
            parentStack.HorizontalOptions = Orientation == StackOrientation.Vertical ? LayoutOptions.Center : HorizontalOptions;
            parentStack.VerticalOptions = VerticalOptions;
            if (Orientation == StackOrientation.Horizontal) parentStack.FlowDirection = Direction;

            var items = ItemsSource;
            foreach (var item in items)
            {
                StackLayout radio = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    BindingContext = item,
                    HorizontalOptions = Orientation == StackOrientation.Horizontal ? LayoutOptions.CenterAndExpand : LayoutOptions.Fill
                };
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += RadioChecked;
                radio.GestureRecognizers.Add(tap);

                var radioId = SelectedValuePath == null ? item.ToString() : item.GetType().GetProperty(SelectedValuePath).GetValue(item, null).ToString();
                var displayText = DisplayMemberPath == null ? item.ToString() : item.GetType().GetProperty(DisplayMemberPath).GetValue(item, null).ToString();

                Label radioText = new Label { Text = displayText, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(20, 0, 0, 0), TextColor = TextColor, FontSize = 10, FontFamily = (Device.RuntimePlatform == Device.Android ? "DroidKufi-Regular.ttf#iconfont" : "DroidArabicKufi") };
                Label radioCircle = new Label { AutomationId = radioId, ClassId = "r", Text = unchckd, TextColor = RadioColor, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center };
                InitDirection(radio, radioText, radioCircle);
            }

            lbRadios = parentStack.Children.Where(x => x is StackLayout).SelectMany(x => ((StackLayout)x).Children.Where(l => l.ClassId == "r").Cast<Label>()).ToList();

            if (SelectedIndex >= 0)
            {
                try
                {
                    lbRadios[SelectedIndex].Text = chckd;
                    lbRadios[SelectedIndex].FontSize = lbRadios[SelectedIndex].FontSize + 10;
                }
                catch (ArgumentOutOfRangeException)
                {
                    SetValue(SelectedIndexProperty, 0);
                    lbRadios[SelectedIndex].Text = chckd;
                    lbRadios[SelectedIndex].FontSize = lbRadios[SelectedIndex].FontSize + 10;
                }
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnItemsAdded?.Invoke(this, new ItemsAddedEventArgs(ItemsSource));
        }

        private void OnItemsSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnItemsAdded?.Invoke(this, new ItemsAddedEventArgs(ItemsSource));
        }

        private void InitDirection(StackLayout radio, Label radioText, Label radioCircle)
        {
            if (Direction == FlowDirection.RightToLeft)
            {
                radioText.HorizontalOptions = LayoutOptions.EndAndExpand;
                radioCircle.HorizontalOptions = LayoutOptions.StartAndExpand;
                radio.Children.Add(radioCircle);
                radio.Children.Add(radioText);
            }
            else
            {
                radioText.HorizontalOptions = LayoutOptions.StartAndExpand;
                radioCircle.HorizontalOptions = LayoutOptions.EndAndExpand;
                radio.Children.Add(radioText);
                radio.Children.Add(radioCircle);
            }
            parentStack.Children.Add(radio);
        }

        private void RadioChecked(object sender, EventArgs e)
        {
            StackLayout stRadio = (StackLayout)sender;
            var lbRadio = stRadio.Children.First(x => x.ClassId == "r") as Label;
            if (lbRadio.Text == unchckd)
            {
                if (SelectedIndex >= 0)
                {
                    lbRadios.Single(x => x.Text == chckd).FontSize = lbRadios.Single(x => x.Text == chckd).FontSize - 10;
                    lbRadios.Single(x => x.Text == chckd).Text = unchckd;
                }
                lbRadio.Text = chckd;
                lbRadio.FontSize = lbRadio.FontSize + 10;

                SelectedItem = stRadio.BindingContext;
                SelectedValue = SelectedValuePath == null ? null : SelectedItem.GetType().GetProperty(SelectedValuePath).GetValue(SelectedItem, null);
                SelectedIndex = ItemsSource.ToList().IndexOf(SelectedItem);
                OnSelectionChanged?.Invoke(this, new SelectionChangedEventArgs(SelectedItem, SelectedValue, SelectedIndex));
            }
        }

    }
}