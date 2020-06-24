using System;
using System.Linq;
using Xamarin.Forms;
using Kangaroo.ViewModels;
using System.Collections.ObjectModel;
using Kangaroo.Resources;
using Kangaroo.Models;
using System.Collections.Generic;

namespace Kangaroo.Controls
{
    public partial class FilterDayCares : BasePage
    {

        #region Declarations
        public event EventHandler<FilterModel> OnApplyFilter;
        #endregion

        #region Functions
        public FilterDayCares(ObservableCollection<CityModel> lstCities)
        {
            InitializeComponent();

            var vm = new FilterViewModel();
            lstCities.Remove(lstCities.Where(a => a.id == "").FirstOrDefault());
            vm.lstData = vm.lstResult = lstCities;
            barTitle.Title = AppResources.lbFilterDayCares;
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

        private void cmdApply_Tapped(object sender, EventArgs e)
        {
            try
            {
                var vm = this.BindingContext as FilterViewModel;
                var oFilterModel = new FilterModel();
                oFilterModel.is_special_needs = (vm.is_special_needs ? "1" : "0");
                oFilterModel.lstSelectedCities = vm.lstResult.Where(a => a.is_selected == true).ToList();
                this.CloseAllPopup();
                OnApplyFilter.Invoke(sender, oFilterModel);
            }
            catch { }
        }

        private void cmdCancel_Tapped(object sender, EventArgs e)
        {
            try
            {
                this.CloseAllPopup();
                OnApplyFilter.Invoke(sender, null);
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = this.BindingContext as FilterViewModel;
            if (string.IsNullOrEmpty(txtSearch.Text)) vm.lstResult = vm.lstData;
            else vm.lstResult = new ObservableCollection<CityModel>(vm.lstData.Where(a => a.city_name.Contains(txtSearch.Text)).ToList());
            this.BindingContext = vm;
        }
        #endregion
        
        
    }

    public class FilterModel
    {
        public string is_special_needs { get; set; }

        public List<CityModel> lstSelectedCities { get; set; }
    }

    public class FilterViewModel : BaseViewModel
    {
        #region Declarations
        private ObservableCollection<CityModel> _lstData;
        private ObservableCollection<CityModel> _lstResult;
        #endregion

        #region Properties
        public bool is_special_needs { get; set; }

        public ObservableCollection<CityModel> lstData
        {
            get { return _lstData; }
            set
            {
                SetProperty(ref _lstData, value);
            }
        }

        public ObservableCollection<CityModel> lstResult
        {
            get { return _lstResult; }
            set
            {
                SetProperty(ref _lstResult, value);
            }
        }
        #endregion

        #region Functions
        public FilterViewModel()
        {

        }
        #endregion
    }
}
