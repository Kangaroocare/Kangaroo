using Com.OneSignal;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.Views;
using Newtonsoft.Json;
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

    public class UserViewModel : BaseViewModel
    {

        #region Declarations
        private UserModel _UserData;

        private List<ListItemModel> _lstGender;
        private List<RelationModel> _lstRelations;
        private List<CountryModel> _lstCountries;
        private List<CityModel> _lstCities;

        private ListItemModel _selected_gender;
        private RelationModel _selected_relation;
        private CountryModel _selected_country;
        private CityModel _selected_city;
        #endregion

        #region Properties
        public UserModel UserData
        {
            get { return _UserData; }
            set { SetProperty(ref _UserData, value); }
        }

        public List<ListItemModel> lstGender
        {
            get { return _lstGender; }
            set { SetProperty(ref _lstGender, value); }
        }

        public List<RelationModel> lstRelations
        {
            get { return _lstRelations; }
            set { SetProperty(ref _lstRelations, value); }
        }

        public List<CountryModel> lstCountries
        {
            get { return _lstCountries; }
            set { SetProperty(ref _lstCountries, value); }
        }

        public List<CityModel> lstCities
        {
            get { return _lstCities; }
            set { SetProperty(ref _lstCities, value); }
        }

        public ListItemModel selected_gender
        {
            get { return _selected_gender; }
            set { SetProperty(ref _selected_gender, value); }
        }

        public RelationModel selected_relation
        {
            get { return _selected_relation; }
            set { SetProperty(ref _selected_relation, value); }
        }

        public CountryModel selected_country
        {
            get { return _selected_country; }
            set
            {
                SetProperty(ref _selected_country, value);
                onChangeCountry(value.country_id);
            }
        }

        public CityModel selected_city
        {
            get { return _selected_city; }
            set { SetProperty(ref _selected_city, value); }
        }

        public ICommand SaveUserCommand { get; }

        public ICommand UpdateProfileCommand { get; }
        #endregion

        #region Functions
        public UserViewModel()
        {
            UserData = new UserModel();

            SaveUserCommand = new Command(OnSaveUser);
            UpdateProfileCommand = new Command(OnUpdateProfile);
        }

        public UserViewModel(INavigation navigation)
        {
            Navigation = navigation;

            lstGender = new List<ListItemModel>();
            lstGender.Add(new ListItemModel() { id = "1", name = AppResources.lbMale });
            lstGender.Add(new ListItemModel() { id = "2", name = AppResources.lbFemale });
            selected_gender = lstGender[0];

            lstRelations = new List<RelationModel>();
            lstCountries = new List<CountryModel>();
            lstCities = new List<CityModel>();

            SaveUserCommand = new Command(OnSaveUser);
            UpdateProfileCommand = new Command(OnUpdateProfile);
        }

        public async void OnLoadUserData()
        {
            try
            {
                IsBusy = true;

                await OnGetAllRelations();
                await OnGetAllCountries();
                await OnGetUserById(Utility.GetInteger(Settings.UserData.id));

                UserData = new UserModel();
                if (Settings.UserData != null && !string.IsNullOrEmpty(Settings.UserId))
                {
                    Settings.UserData.confirm_email = Settings.UserData.email;
                    UserData = Settings.UserData;
                    selected_gender = lstGender.Where(a => a.id == UserData.gender).FirstOrDefault();
                    selected_relation = lstRelations.Where(a => a.id == UserData.child_relation_id).FirstOrDefault();
                    selected_country = lstCountries.Where(a => a.country_id == UserData.country_id).FirstOrDefault();
                    await OnGetCountryCities(selected_country.country_id);
                    selected_city = lstCities.Where(a => a.id == UserData.city_id).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                //Log.Report(ex);
                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnSaveUser()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (string.IsNullOrEmpty(UserData.full_name)) { await Utility.ShowNotification("", AppResources.msgReqUserName); return; }
                if (string.IsNullOrEmpty(UserData.mobile)) { await Utility.ShowNotification("", AppResources.msgReqMobile); return; }
                if (UserData.mobile.Length != 9) { await Utility.ShowNotification("", AppResources.msgInvalidMobileNo); return; }
                if (string.IsNullOrEmpty(UserData.password)) { await Utility.ShowNotification("", AppResources.msgReqPassword); return; }
                if (string.IsNullOrEmpty(UserData.confirm_password)) { await Utility.ShowNotification("", AppResources.msgReqConfirmPassword); return; }
                if (UserData.password != UserData.confirm_password) { await Utility.ShowNotification("", AppResources.msgConfirmPasswordError); return; }

                IsBusy = true;
                string url = "api/users/register";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "password", ParameterValue = UserData.password.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "full_name", ParameterValue = UserData.full_name.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = UserData.mobile.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "call_key", ParameterValue = "+966" });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_type", ParameterValue = "4" });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<UserResult>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgAccountCreatedSuccessfully);
                    await Application.Current.MainPage.Navigation.PopAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(new ActivationPage(Utility.GetInteger(oResult.data.id), oResult.data.mobile));
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

        private async void OnUpdateProfile()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (string.IsNullOrEmpty(UserData.national_id)) { await Utility.ShowNotification("", AppResources.msgReqNationalId); return; }
                if (string.IsNullOrEmpty(UserData.full_name)) { await Utility.ShowNotification("", AppResources.msgReqUserName); return; }
                if (string.IsNullOrEmpty(UserData.mobile)) { await Utility.ShowNotification("", AppResources.msgReqMobile); return; }
                if (string.IsNullOrWhiteSpace(UserData.mobile) || UserData.mobile.Length != 9) { await Utility.ShowNotification("", AppResources.msgInvalidMobileNo); return; }
                if (selected_relation == null) { await Utility.ShowNotification("", AppResources.msgReqChildRelation); return; }
                if (!string.IsNullOrWhiteSpace(UserData.email))
                {
                    bool isEmail = Regex.IsMatch(UserData.email, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})", RegexOptions.IgnoreCase);
                    if (!isEmail) { await Utility.ShowNotification("", AppResources.msgEmailFormatError); return; }
                }
                if (UserData.confirm_email != UserData.email) { await Utility.ShowNotification("", AppResources.msgConfirmEmailError); return; }
                if (selected_country == null) { await Utility.ShowNotification("", AppResources.msgReqCountry); return; }
                if (selected_city == null) { await Utility.ShowNotification("", AppResources.msgReqCity); return; }

                IsBusy = true;
                string url = "api/users/change_profile";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "full_name", ParameterValue = UserData.full_name.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "call_key", ParameterValue = "+966" });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = UserData.mobile });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = UserData.id.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "email", ParameterValue = UserData.email.Trim() });
                //lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = "2" });
                lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = selected_country.country_id.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "city_id", ParameterValue = selected_city.id.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "national_id", ParameterValue = UserData.national_id.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "gender", ParameterValue = selected_gender.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_type", ParameterValue = "4" });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_relation_id", ParameterValue = selected_relation.id.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "job_role_id", ParameterValue = "" });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<UserResult>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgProfileUpdatedSuccessfully);
                    Settings.UserData = oResult.data;
                    Settings.jsUserData = JsonConvert.SerializeObject(Settings.UserData);
                    if (Settings.UserData.mobile_activated == "0") await App.Current.MainPage.Navigation.PushAsync(new ActivationPage(Utility.GetInteger(oResult.data.id), oResult.data.mobile));
                    else await App.Current.MainPage.Navigation.PushAsync(new HomePage());
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

        public async Task OnActivate(int userId, string mobileNo, string activationCode)
        {
            try
            {
                IsBusy = true;
                string url = "api/users/activate_user";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = mobileNo.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "activation_code", ParameterValue = activationCode.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "call_key", ParameterValue = "+966" });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgActivationCompleted);
                    await OnGetUserById(userId);
                    OneSignal.Current.SendTag("user_id", Settings.UserId);
                    await Application.Current.MainPage.Navigation.PopAsync();
                    await Application.Current.MainPage.Navigation.PopAsync();
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

        public async Task OnGetUserById(int userId)
        {
            try
            {
                IsBusy = true;
                string url = "api/users/get_user_by_id";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = userId.ToString() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<UserResult>(json);
                if (oResult.response_status == "200")
                {
                    Settings.UserData = oResult.data;
                    Settings.jsUserData = JsonConvert.SerializeObject(oResult.data);
                    Settings.UserId = oResult.data.id.ToString();
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

        public async Task OnResend(int userId)
        {
            try
            {
                IsBusy = true;
                string url = "api/users/resend_sms";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = userId.ToString() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200") await Utility.ShowNotification("", AppResources.msgResendCompleted);
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

        public async Task OnChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(oldPassword)) { await Utility.ShowNotification("", AppResources.msgReqOldPassword); return; }
                if (string.IsNullOrEmpty(newPassword)) { await Utility.ShowNotification("", AppResources.msgReqNewPassword); return; }
                if (string.IsNullOrEmpty(confirmPassword)) { await Utility.ShowNotification("", AppResources.msgReqConfirmPassword); return; }
                if (confirmPassword != newPassword) { await Utility.ShowNotification("", AppResources.msgConfirmPasswordError); return; }

                IsBusy = true;
                string url = "api/users/renew_password";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = Settings.UserData.mobile.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "old_password", ParameterValue = oldPassword.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "new_password", ParameterValue = newPassword.Trim() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgPasswordChanged);
                    Settings.UserId = null;
                    Settings.UserData = null;
                    Settings.jsUserData = string.Empty;
                    await Application.Current.MainPage.Navigation.PopAsync();
                    await Application.Current.MainPage.Navigation.PopAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                else if (oResult.response_status == "600")
                {
                    await Utility.ShowNotification("", AppResources.msgOldPasswordInCorrect);
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

        public async Task OnGetAllRelations()
        {
            try
            {
                string url = "api/settings/get_relations";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<RelationListResult>(json);
                if (oResult.response_status == "200") lstRelations = oResult.data;
                else await Utility.ShowNotification("", oResult.response_message);
            }
            catch (Exception ex)
            {
                //Log.Report(ex);
                await Utility.ShowNotification("", ex.Message);
            }
        }

        public async Task OnGetAllCountries()
        {
            try
            {
                string url = "api/settings/get_countries";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<CountryListResult>(json);
                if (oResult.response_status == "200")
                {
                    lstCountries = oResult.data;
                    //selected_country = lstCountries.Where(a => a.country_id == "2").FirstOrDefault();
                }
                else await Utility.ShowNotification("", oResult.response_message);
            }
            catch (Exception ex)
            {
                //Log.Report(ex);
                await Utility.ShowNotification("", ex.Message);
            }
        }

        public async void onChangeCountry(string countryId)
        {
            await OnGetCountryCities(countryId);
        }

        public async Task OnGetCountryCities(string countryId)
        {
            try
            {
                IsBusy = true;
                string url = "api/settings/get_cities";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = countryId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                selected_city = null;
                var oResult = JsonConvert.DeserializeObject<CityListResult>(json);
                if (oResult.response_status == "200") lstCities = oResult.data;
                else lstCities = new List<CityModel>();
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
        #endregion

    }
}
