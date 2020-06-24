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
    public class ContactUsViewModel : BaseViewModel
    {

        #region Declarations
        private string _contact_name;
        private string _contact_title;
        private string _contact_email;
        private string _contact_subject;
        #endregion

        #region Properties
        public string contact_name
        {
            get { return _contact_name; }
            set { SetProperty(ref _contact_name, value); }
        }

        public string contact_title
        {
            get { return _contact_title; }
            set { SetProperty(ref _contact_title, value); }
        }

        public string contact_email
        {
            get { return _contact_email; }
            set { SetProperty(ref _contact_email, value); }
        }

        public string contact_subject
        {
            get { return _contact_subject; }
            set { SetProperty(ref _contact_subject, value); }
        }

        public ICommand SendMessageCommand { get; private set; }
        #endregion

        #region Functions
        public ContactUsViewModel(INavigation navigation)
        {
            Navigation = navigation;

            SendMessageCommand = new Command(OnContactUs);

            if (Settings.UserData != null)
            {
                contact_name = Settings.UserData.full_name;
                contact_email = Settings.UserData.email;
            }
        }

        private async void OnContactUs()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (string.IsNullOrEmpty(contact_name)) { await Utility.ShowNotification("", AppResources.msgReqContactName); return; }
                if (string.IsNullOrEmpty(contact_title)) { await Utility.ShowNotification("", AppResources.msgReqContactTitle); return; }
                if (string.IsNullOrEmpty(contact_email)) { await Utility.ShowNotification("", AppResources.msgReqContactEmail); return; }
                if (string.IsNullOrEmpty(contact_subject)) { await Utility.ShowNotification("", AppResources.msgReqContactSubject); return; }

                IsBusy = true;
                string url = "api/contact_us/send";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "contact_name", ParameterValue = contact_name });
                lstParamters.Add(new ApiParameters() { ParameterName = "contact_title", ParameterValue = contact_title });
                lstParamters.Add(new ApiParameters() { ParameterName = "contact_email", ParameterValue = contact_email });
                lstParamters.Add(new ApiParameters() { ParameterName = "contact_subject", ParameterValue = contact_subject });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgMessageSentSuccessfully);
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
                IsTapped = false;
                IsBusy = false;
            }
        }
        #endregion

    }
}
