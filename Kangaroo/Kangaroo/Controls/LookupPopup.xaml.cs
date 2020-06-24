using System;
using System.Linq;
using Xamarin.Forms;
using Kangaroo.ViewModels;
using System.Collections.ObjectModel;

namespace Kangaroo.Controls
{
    public partial class LookupPopup : BasePage
    {

        #region Declarations
        public event EventHandler<LookupModel> OnSelectItem;
        #endregion

        #region Functions
        public LookupPopup(ObservableCollection<LookupModel> lstInitialData, string title)
        {
            InitializeComponent();

            var vm = new LookupViewModel();
            lstInitialData.Remove(lstInitialData.Where(a => a.id == "").FirstOrDefault());
            vm.lstData = vm.lstResult = lstInitialData;
            barTitle.Title = title;
            this.BindingContext = vm;
        }

        private async void CloseAllPopup()
        {
            await Navigation.PopAsync();
        }
        #endregion

        #region Events
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private void PopupPage_Disappearing(object sender, EventArgs e)
        {

        }

        private void OnTapped(object sender, EventArgs e)
        {
            try
            {
                var tap = e as TappedEventArgs;
                var vm = this.BindingContext as LookupViewModel;
                var oLookup = tap.Parameter as LookupModel;
                this.CloseAllPopup();
                OnSelectItem.Invoke(sender, oLookup);
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = this.BindingContext as LookupViewModel;
            if (string.IsNullOrEmpty(txtSearch.Text)) vm.lstResult = vm.lstData;
            else vm.lstResult = new ObservableCollection<LookupModel>(vm.lstData.Where(a => a.description.Contains(txtSearch.Text)).ToList());
            this.BindingContext = vm;
        }
        #endregion
    }

    public class LookupModel
    {
        public string id { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public bool show_icon { get; set; }
    }

    public class LookupViewModel : BaseViewModel
    {
        #region Declarations
        private ObservableCollection<LookupModel> _lstData;
        private ObservableCollection<LookupModel> _lstResult;
        #endregion

        #region Properties
        public ObservableCollection<LookupModel> lstData
        {
            get { return _lstData; }
            set
            {
                SetProperty(ref _lstData, value);
            }
        }

        public ObservableCollection<LookupModel> lstResult
        {
            get { return _lstResult; }
            set
            {
                SetProperty(ref _lstResult, value);
            }
        }
        #endregion

        #region Functions
        public LookupViewModel()
        {

        }
        #endregion
    }
}
