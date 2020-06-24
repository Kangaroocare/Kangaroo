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
    public class ReportViewModel : BaseViewModel
    {

        #region Declarations
        private ReportModel _ReportModel;

        private List<ActivityTypeModel> _lstActivityTypes;

        private int _current_view_index;
        private string _current_view_title;
        private ContentView _current_view;
        #endregion

        #region Properties
        public ReportModel ReportModel
        {
            get { return _ReportModel; }
            set { SetProperty(ref _ReportModel, value); }
        }

        public List<ActivityTypeModel> lstActivityTypes
        {
            get { return _lstActivityTypes; }
            set { SetProperty(ref _lstActivityTypes, value); }
        }

        public ICommand PreviousCommand { get; }

        public ICommand NextCommand { get; }

        public ICommand BackCommand { get; }

        public ICommand OpenFileCommand { get; }

        public int current_view_index
        {
            get { return _current_view_index; }
            set { SetProperty(ref _current_view_index, value); }
        }

        public string current_view_title
        {
            get { return _current_view_title; }
            set { SetProperty(ref _current_view_title, value); }
        }

        public ContentView current_view
        {
            get { return _current_view; }
            set { SetProperty(ref _current_view, value); }
        }
        #endregion

        #region Functions
        public ReportViewModel(INavigation navigation)
        {
            Navigation = navigation;

            ReportModel = new ReportModel();
            lstActivityTypes = new List<ActivityTypeModel>();

            PreviousCommand = new Command(OnPreviousView);
            NextCommand = new Command(OnNextView);
            BackCommand = new Command(OnBack);
            OpenFileCommand = new Command<FileModel>(OnOpenFile);

            current_view_index = 0;
            current_view_title = AppResources.lbDailyReportActivity;
            current_view = new ReportActivityView();
        }

        private void OnPreviousView()
        {
            try
            {
                switch (current_view_index)
                {
                    case 1:
                        current_view = new ReportActivityView();
                        current_view_title = AppResources.lbDailyReportActivity;
                        break;
                }
                current_view_index -= 1;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void OnNextView()
        {
            try
            {
                switch (current_view_index)
                {
                    case 0:
                        current_view = new ReportMediaView();
                        current_view_title = AppResources.lbDailyReportMedia;
                        break;
                }
                current_view_index += 1;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void OnBack()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public async void OnOpenFile(FileModel reportFile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }

            if (reportFile.is_image == "0") await Application.Current.MainPage.Navigation.PushAsync(new VideoPlayerPage(reportFile.file));
            else await Application.Current.MainPage.Navigation.PushAsync(new ImageViewerPage(reportFile.file));

            IsTapped = false;
        }

        public async Task OnGetActivityTypes()
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/get_daily_reports_activity_types";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    IsBusy = false;
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ActivityTypeListResult>(json);
                if (oResult.response_status == "200") lstActivityTypes = oResult.data;
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

        public async void OnGetReportData(string reportId)
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/get_daily_report_by_id";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "daily_report_id", ParameterValue = reportId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ReportResult>(json);
                if (oResult.response_status == "200")
                {
                    await OnGetActivityTypes();

                    foreach (var oActivity in oResult.data.activities)
                    {
                        if (oActivity.activity_type_id == "1" || oActivity.activity_type_id == "3" || oActivity.activity_type_id == "4")
                        {
                            oActivity.is_default_item = true;
                            oActivity.activity_time = oActivity.activity_time.Substring(0, 5);
                        }
                        else if (oActivity.activity_type_id == "2")
                        {
                            oActivity.is_nutrition_item = true;
                            oActivity.activity_time = oActivity.activity_time.Substring(0, 5);
                        }
                        else if (oActivity.activity_type_id == "5") oActivity.is_supply_item = true;
                    }

                    int nSerial = 1;
                    foreach (var oFile in oResult.data.report_files)
                    {
                        oFile.serial = nSerial;
                        nSerial++;
                    }

                    // Activity Types Headers
                    foreach (var oActivityType in lstActivityTypes)
                    {
                        var oActivityModel = new ActivityModel();

                        oActivityModel.activity_type_id = oActivityType.activity_id;
                        oActivityModel.activity_type_name = oActivityType.activity_type;
                        oActivityModel.sort = 0;

                        if (oActivityType.activity_id == "2") oActivityModel.is_nutrition_header = true;
                        else if (oActivityType.activity_id == "5") oActivityModel.is_supply_header = true;
                        else oActivityModel.is_default_header = true;

                        oResult.data.activities.Add(oActivityModel);
                    }

                    oResult.data.activities = oResult.data.activities.OrderBy(a => a.activity_type_id).ThenBy(a => a.sort).ToList();
                    ReportModel = oResult.data;
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
        #endregion

    }
}
