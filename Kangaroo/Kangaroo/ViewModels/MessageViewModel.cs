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
    public class MessageViewModel : BaseViewModel
    {

        #region Declarations
        private List<MessageModel> _lstMessageDetails;
        #endregion

        #region Properties
        public List<MessageModel> lstMessageDetails
        {
            get { return _lstMessageDetails; }
            set { SetProperty(ref _lstMessageDetails, value); }
        }
        #endregion

        #region Functions
        public MessageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            lstMessageDetails = new List<MessageModel>();
        }

        public async Task OnGetMessageDetails(string recipientId)
        {
            try
            {
                IsBusy = true;
                string url = "api/messages/get_message_details/";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "sender_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "recipient_id", ParameterValue = recipientId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<MessageResult>(json);
                if (oResult.response_status == "200") lstMessageDetails = oResult.data;
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

        public async Task OnSendReplyMessage(string recipientId, string newReply)
        {
            try
            {
                if (string.IsNullOrEmpty(newReply))
                {
                    await Utility.ShowNotification("", AppResources.msgReqMessageText);
                    return;
                }

                IsBusy = true;
                string url = "api/messages/send_reply_message/";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "sender_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "recipient_id", ParameterValue = recipientId });
                lstParamters.Add(new ApiParameters() { ParameterName = "new_reply", ParameterValue = newReply });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status != "200") await Utility.ShowNotification("", oResult.response_message);
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
