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
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kangaroo.ViewModels
{

    public class ClassViewModel : BaseViewModel
    {

        #region Declarations

        #endregion

        #region Properties

        #endregion

        #region Functions
        public ClassViewModel()
        {

        }

        public async void OnRecordAttendance(string classId, string childId, bool isAttend)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/teacher_panel/record_attendance";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "class_id", ParameterValue = classId });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = childId });
                lstParamters.Add(new ApiParameters() { ParameterName = "is_attend", ParameterValue = (isAttend ? "1" : "0") });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                //var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                //if (oResult.response_status != "200") await Utility.ShowNotification("", oResult.response_message);
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