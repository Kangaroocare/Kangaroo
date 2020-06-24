using Com.OneSignal;
using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kangaroo.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {

        #region Declarations
        private string _user_login;
        private string _password;
        private string _country_id;
        private string _country_name;
        private string _country_key;
        private string _call_key;
        private string _mobile_ex;
        private string _flag;
        private string _flag_path;
        #endregion

        #region Properties
        public string user_login
        {
            get { return _user_login; }
            set { SetProperty(ref _user_login, value); }
        }

        public string password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string country_id
        {
            get { return _country_id; }
            set { SetProperty(ref _country_id, value); }
        }

        public string country_name
        {
            get { return _country_name; }
            set { SetProperty(ref _country_name, value); }
        }

        public string country_key
        {
            get { return _country_key; }
            set { SetProperty(ref _country_key, value); }
        }

        public string call_key
        {
            get { return _call_key; }
            set { SetProperty(ref _call_key, value); }
        }

        public string mobile_ex
        {
            get { return _mobile_ex; }
            set { SetProperty(ref _mobile_ex, value); }
        }

        public string flag
        {
            get { return _flag; }
            set { SetProperty(ref _flag, value); }
        }

        public string flag_path
        {
            get { return _flag_path; }
            set { SetProperty(ref _flag_path, value); }
        }

        public ICommand LoginCommand { get; private set; }

        public ICommand SearchCountryCommand { get; private set; }
        #endregion

        #region Functions
        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;

            LoginCommand = new Command(OnLogin);
            SearchCountryCommand = new Command(OnSearchCountry);
            OnGetSiteSettings();

            //user_login = "544957281";
            //password = "123456";

            //user_login = "0555055475"; // Customer Mobile
            //password = "123456";
        }

        private async void OnLogin()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (string.IsNullOrEmpty(user_login) || string.IsNullOrEmpty(password))
                {
                    await Utility.ShowNotification("", AppResources.msgReqLoginData);
                    return;
                }

                IsBusy = true;
                string url = "api/users/login";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_login", ParameterValue = user_login.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "password", ParameterValue = password.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "call_key", ParameterValue = call_key.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = country_id.Trim() });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<UserResult>(json);
                if (oResult.response_status == "200")
                {
                    if (oResult.data.mobile_activated == "0")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new ActivationPage(Utility.GetInteger(oResult.data.id), oResult.data.mobile));
                        await Utility.ShowNotification("", AppResources.msgReqActivationCode);
                    }
                    else
                    {
                        Settings.UserData = oResult.data;
                        Settings.jsUserData = JsonConvert.SerializeObject(oResult.data);
                        Settings.UserId = oResult.data.id.ToString();
                        OneSignal.Current.SendTag("user_id", Settings.UserId);
                        Settings.HomeTabIndex = 0;

                        await Application.Current.MainPage.Navigation.PopAsync();
                        string msg = string.Format("{0} {1}", AppResources.lbWelcome, oResult.data.full_name);
                        await Utility.ShowNotification("", msg);
                    }
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

        public async Task OnForgetPassword(string mobileNo)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileNo)) { await Utility.ShowNotification("", AppResources.msgReqMobile); return; }

                IsBusy = true;
                string url = "api/users/reset_password_code";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = mobileNo.Trim() });
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
                    await Utility.ShowNotification("", oResult.response_message);
                    await Application.Current.MainPage.Navigation.PushAsync(new ResetPasswordPage());
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

        public async Task OnResetPassword(string mobileNo, string resetCode, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileNo)) { await Utility.ShowNotification("", AppResources.msgReqMobile); return; }
                if (string.IsNullOrEmpty(resetCode)) { await Utility.ShowNotification("", AppResources.msgReqResetCode); return; }
                if (string.IsNullOrEmpty(newPassword)) { await Utility.ShowNotification("", AppResources.msgReqNewPassword); return; }

                IsBusy = true;
                string url = "api/users/reset_password";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "mobile", ParameterValue = mobileNo.Trim() });
                lstParamters.Add(new ApiParameters() { ParameterName = "reset_code", ParameterValue = resetCode.Trim() });
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
                    await Utility.ShowNotification("", oResult.response_message);
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
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

        public async void OnGetSiteSettings()
        {
            try
            {
                IsBusy = true;
                string url = "api/settings/get_site_settings";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<SettingResult>(json);
                if (oResult.response_status == "200")
                {
                    var oDefaultCountry = oResult.data.default_country;
                    country_id = oDefaultCountry.country_id;
                    country_name = oDefaultCountry.country_name;
                    country_key = oDefaultCountry.country_key;
                    call_key = oDefaultCountry.call_key;
                    mobile_ex = oDefaultCountry.mobile_ex;
                    flag = oDefaultCountry.flag;
                    flag_path = Utility.ServerUrl + oDefaultCountry.flag;
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

        public async void OnSearchCountry()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
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
                    var lstData = new ObservableCollection<LookupModel>();
                    foreach (var data in oResult.data)
                    {
                        lstData.Add(new LookupModel()
                        {
                            id = data.country_id.ToString(),
                            description = data.country_key,
                            icon = Utility.ServerUrl + data.flag,
                            show_icon = true
                        });
                    }
                    var oSearch = new Controls.LookupPopup(lstData, AppResources.lbSelectCountry);
                    await Application.Current.MainPage.Navigation.PushAsync(oSearch);
                    oSearch.OnSelectItem += OnSelectCountry;
                    await Task.Delay(1000);
                    IsTapped = false;
                }
            }
            catch (Exception ex)
            {
                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsBusy = false;
                IsTapped = false;
            }
        }

        private async void OnSelectCountry(object sender, LookupModel selectedCountry)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (selectedCountry != null && country_id != selectedCountry.id)
                {
                    IsBusy = true;
                    string url = "api/user_panel/get_country_by_id";
                    var lstParamters = new List<ApiParameters>();
                    lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                    lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = selectedCountry.id });

                    string json = await Utility.CallWebApi(lstParamters, url);
                    if (json == null)
                    {
                        await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                        return;
                    }

                    var oResult = JsonConvert.DeserializeObject<CountryResult>(json);
                    if (oResult.response_status == "200")
                    {
                        var oCountry = oResult.data;
                        country_id = oCountry.country_id;
                        country_name = oCountry.country_name;
                        country_key = oCountry.country_key;
                        call_key = oCountry.call_key;
                        mobile_ex = oCountry.mobile_ex;
                        flag_path = Utility.ServerUrl + oCountry.flag;
                    }
                    else
                    {
                        country_id = "0";
                        country_name = AppResources.lbSelectCountry;
                        country_key = string.Empty;
                        call_key = string.Empty;
                        mobile_ex = AppResources.lbMobileWithHint;
                        flag_path = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                country_id = "0";
                country_name = AppResources.lbSelectCountry;
                country_key = string.Empty;
                call_key = string.Empty;
                mobile_ex = AppResources.lbMobileWithHint;
                flag_path = string.Empty;

                await Utility.ShowNotification("", ex.Message);
            }
            finally
            {
                IsTapped = false;
                IsBusy = false;
            }
        }
        #endregion

    }
}
