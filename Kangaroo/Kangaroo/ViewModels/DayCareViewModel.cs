using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kangaroo.ViewModels
{
    public class DayCareViewModel : BaseViewModel
    {

        #region Declarations
        private int _holiday_height;

        private int _current_page_favorite_daycares;
        private bool _load_more_favorite_daycares;

        private DayCareDetailsModel _DayCareDetails;
        private ObservableCollection<DayCareModel> _lstFavoriteDayCares;
        #endregion

        #region Properties
        public int holiday_height
        {
            get { return _holiday_height; }
            set { SetProperty(ref _holiday_height, value); }
        }

        public int current_page_favorite_daycares
        {
            get { return _current_page_favorite_daycares; }
            set { SetProperty(ref _current_page_favorite_daycares, value); }
        }

        public bool load_more_favorite_daycares
        {
            get { return _load_more_favorite_daycares; }
            set { SetProperty(ref _load_more_favorite_daycares, value); }
        }

        public DayCareDetailsModel DayCareDetails
        {
            get { return _DayCareDetails; }
            set { SetProperty(ref _DayCareDetails, value); }
        }

        public ObservableCollection<DayCareModel> lstFavoriteDayCares
        {
            get { return _lstFavoriteDayCares; }
            set { SetProperty(ref _lstFavoriteDayCares, value); }
        }

        public ICommand FavoriteDayCaresCommand { get; private set; }

        public ICommand ContactDayCareCommand { get; private set; }

        public ICommand RemoveFavoriteCommand { get; private set; }

        public ICommand DayCareDetailsCommand { get; private set; }

        public ICommand CallDayCareCommand { get; private set; }
        #endregion

        #region Functions
        public DayCareViewModel(INavigation navigation)
        {
            Navigation = navigation;

            holiday_height = 0;

            current_page_favorite_daycares = 0;
            load_more_favorite_daycares = true;

            lstFavoriteDayCares = new ObservableCollection<DayCareModel>();

            FavoriteDayCaresCommand = new Command(OnGetFavoriteDayCares);
            ContactDayCareCommand = new Command<DayCareModel>(OnContactDayCare);
            RemoveFavoriteCommand = new Command<string>(OnRemoveFavorite);
            DayCareDetailsCommand = new Command<string>(OnGetDayCareDetails);
            CallDayCareCommand = new Command<string>(OnCallDayCare);
        }

        public async void OnGetFavoriteDayCares()
        {
            try
            {
                if (!load_more_favorite_daycares) return;
                current_page_favorite_daycares++;

                IsBusy = true;
                string url = "api/parent_panel/my_favorites";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "current_page", ParameterValue = current_page_favorite_daycares.ToString() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<DayCareListResult>(json);
                if (oResult.response_status == "200")
                {
                    if (oResult.data != null && oResult.data.Count > 0)
                    {
                        load_more_favorite_daycares = (oResult.data.Count >= 6 ? true : false);
                        foreach (var oDayCare in oResult.data)
                        { lstFavoriteDayCares.Add(oDayCare); }
                    }
                    else load_more_favorite_daycares = false;
                }
                else if (current_page_favorite_daycares == 1) await Utility.ShowNotification("", oResult.response_message);
            }
            catch (Exception ex)
            {
                //Log.Report(ex);
                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnContactDayCare(DayCareModel daycare)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(daycare.school_admin_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedDayCareAdminId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(daycare.school_admin_id, daycare.daycare_name));
            IsTapped = false;
        }

        public async void OnRemoveFavorite(string daycareId)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/parent_panel/add_favorite";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "daycare_id", ParameterValue = daycareId });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    var oDayCare = lstFavoriteDayCares.Where(a => a.daycare_id == daycareId).FirstOrDefault();
                    if (oDayCare != null) lstFavoriteDayCares.Remove(oDayCare);
                }
                else await Utility.ShowNotification("", oResult.response_message);
            }
            catch (Exception ex)
            {
                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsTapped = false;
                IsBusy = false;
            }
        }

        public async void OnGetDayCareDetails(string daycareId)
        {
            try
            {
                IsBusy = true;
                string url = "api/daycares/daycare_details";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "daycare_id", ParameterValue = daycareId });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId.Trim() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<DayCareDetailsResult>(json);
                if (oResult.response_status == "200")
                {
                    if (oResult.data.holidays == null || oResult.data.holidays.Count == 0) oResult.data.holidays = new List<HolidayModel>();
                    DayCareDetails = oResult.data;

                    if (oResult.data.holidays.Count == 0) holiday_height = 100;
                    else holiday_height = (DayCareDetails.holidays.Count * 90) + 10;
                }
                else await Utility.ShowNotification("", oResult.response_message);
            }
            catch (Exception ex)
            {
                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnCallDayCare(string contactNumber)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (!string.IsNullOrEmpty(contactNumber)) Device.OpenUri(new Uri("tel:" + contactNumber));
            IsTapped = false;
        }
        #endregion

    }
}
