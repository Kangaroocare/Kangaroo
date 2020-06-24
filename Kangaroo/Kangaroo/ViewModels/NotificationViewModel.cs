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
    public class NotificationViewModel : BaseViewModel
    {

        #region Declarations
        private ObservableCollection<NotificationModel> _lstNotifications;
        #endregion

        #region Properties
        public ObservableCollection<NotificationModel> lstNotifications
        {
            get { return _lstNotifications; }
            set { SetProperty(ref _lstNotifications, value); }
        }
        #endregion

        #region Functions
        public NotificationViewModel(INavigation navigation)
        {
            Navigation = navigation;

            lstNotifications = new ObservableCollection<NotificationModel>();
        }

        public async void OnGetNotifications()
        {
            try
            {
                IsBusy = true;
                string url = "api/notifications/get_notifications_list";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<NotificationResult>(json);
                if (oResult.response_status == "200")
                {
                    int index = 1;
                    lstNotifications = new ObservableCollection<NotificationModel>();
                    foreach (var notification in oResult.data)
                    {
                        lstNotifications.Add(notification);
                        index++;
                    }
                }
                //else await Utility.ShowNotification("", oResult.response_message);
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

        public async void OnGetParentNotifications(string parentId)
        {
            try
            {
                IsBusy = true;
                string url = "api/notifications/get_notifications_list";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "parent_id", ParameterValue = parentId });
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<NotificationResult>(json);
                if (oResult.response_status == "200")
                {
                    int index = 1;
                    lstNotifications = new ObservableCollection<NotificationModel>();
                    foreach (var notification in oResult.data)
                    {
                        lstNotifications.Add(notification);
                        index++;
                    }
                }
                //else await Utility.ShowNotification("", oResult.response_message);
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
        #endregion

    }
}
