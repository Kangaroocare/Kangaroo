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
    public class DailyReportViewModel : BaseViewModel
    {

        #region Declarations
        private ObservableCollection<DailyReportModel> _lstChildReports;
        #endregion

        #region Properties
        public ObservableCollection<DailyReportModel> lstChildReports
        {
            get { return _lstChildReports; }
            set { SetProperty(ref _lstChildReports, value); }
        }

        public ICommand ReportDetailsCommand { get; }
        #endregion

        #region Functions
        public DailyReportViewModel(INavigation navigation)
        {
            Navigation = navigation;

            lstChildReports = new ObservableCollection<DailyReportModel>();

            ReportDetailsCommand = new Command<string>(OnOpenReportDetails);
        }

        public async void OnGetChildDailyReports(string childId)
        {
            try
            {
                IsBusy = true;
                string url = "api/parent_panel/get_child_daily_reports";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = childId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<DailyReportListResult>(json);
                if (oResult.response_status == "200")
                {
                    lstChildReports = new ObservableCollection<DailyReportModel>();
                    foreach (var oDailyReport in oResult.data)
                    {
                        lstChildReports.Add(oDailyReport);
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
                IsBusy = false;
            }
        }

        public async void OnOpenReportDetails(string reportId)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(reportId))
            {
                await Utility.ShowNotification("", AppResources.msgMissedReportId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new ReportDetailsPage(reportId));
            IsTapped = false;
        }
        #endregion

    }
}
