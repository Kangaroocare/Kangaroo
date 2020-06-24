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
    public class ChildViewModel : BaseViewModel
    {

        #region Declarations
        private ChildModel _ChildData;

        private ObservableCollection<GalleryModel> _lstChildGallery;
        private ObservableCollection<DayCareModel> _lstDayCares;

        private DayCareModel _selected_daycare;
        #endregion

        #region Properties
        public ChildModel ChildData
        {
            get { return _ChildData; }
            set { SetProperty(ref _ChildData, value); }
        }

        public ObservableCollection<GalleryModel> lstChildGallery
        {
            get { return _lstChildGallery; }
            set { SetProperty(ref _lstChildGallery, value); }
        }

        public ObservableCollection<DayCareModel> lstDayCares
        {
            get { return _lstDayCares; }
            set { SetProperty(ref _lstDayCares, value); }
        }

        public DayCareModel selected_daycare
        {
            get { return _selected_daycare; }
            set { SetProperty(ref _selected_daycare, value); }
        }

        // Parent Commands //
        public ICommand ContactTeacherCommand { get; private set; }

        public ICommand ChildGalleryCommand { get; private set; }

        public ICommand DailyReportCommand { get; private set; }

        public ICommand NewDayCareCommand { get; private set; }

        public ICommand RequestDayCareCommand { get; private set; }

        public ICommand OpenFileCommand { get; private set; }

        // Teacher Commands //
        public ICommand OpenChildProfileCommand { get; private set; }

        public ICommand ContactParentCommand { get; private set; }

        public ICommand CreateReportCommand { get; private set; }

        public ICommand ParentNotificationsCommand { get; private set; }

        public ICommand UploadGalleryCommand { get; private set; }

        public ICommand CallParentCommand { get; private set; }
        #endregion

        #region Functions
        public ChildViewModel(INavigation navigation)
        {
            Navigation = navigation;

            ChildData = new ChildModel();
            lstChildGallery = new ObservableCollection<GalleryModel>();
            lstDayCares = new ObservableCollection<DayCareModel>();

            // Parent Commands //
            ContactTeacherCommand = new Command<ChildModel>(OnContactTeacher);
            ChildGalleryCommand = new Command<ChildModel>(OnOpenChildGallery);
            DailyReportCommand = new Command<ChildModel>(OnOpenChildReports);
            NewDayCareCommand = new Command<ChildModel>(OnNewDayCare);
            RequestDayCareCommand = new Command(OnRequestDayCare);
            OpenFileCommand = new Command<GalleryModel>(OnOpenFile);

            // Teacher Commands //
            OpenChildProfileCommand = new Command<string>(OnOpenChildProfile);
            ContactParentCommand = new Command<ChildModel>(OnContactParent);
            CreateReportCommand = new Command<ChildModel>(OnCreateReport);
            ParentNotificationsCommand = new Command<string>(OnParentNotifications);
            UploadGalleryCommand = new Command<string>(OnUploadGallery);
            CallParentCommand = new Command<string>(OnCallParent);
        }

        public async void OnGetChildData(string childId)
        {
            try
            {
                IsBusy = true;
                string url = "api/parent_panel/my_child";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = childId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ChildResult>(json);
                if (oResult.response_status == "200") ChildData = oResult.data;
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

        public async void OnGetChildById(string childId)
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/get_child_by_id";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = childId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ChildResult>(json);
                if (oResult.response_status == "200") ChildData = oResult.data;
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

        public async void OnGetDayCares()
        {
            try
            {
                IsBusy = true;
                string url = "api/daycares/get_closest_daycares";
                var lstParamters = new List<ApiParameters>();
                await Utility.GetCurrentLocationAsync();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "lat", ParameterValue = Settings.LastKnownLocation.Latitude.ToString() });
                lstParamters.Add(new ApiParameters() { ParameterName = "lng", ParameterValue = Settings.LastKnownLocation.Longitude.ToString() });
                if (!string.IsNullOrEmpty(Settings.UserId)) lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<DayCareListResult>(json);
                if (oResult.response_status == "200")
                {
                    lstDayCares = new ObservableCollection<DayCareModel>();
                    foreach (var oDayCare in oResult.data)
                    {
                        lstDayCares.Add(oDayCare);
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

        // Parent Actions //
        public async void OnContactTeacher(ChildModel child)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(child.teacher_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedTeacherId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(child.teacher_id, child.teacher_name));
            IsTapped = false;
        }

        public async void OnOpenChildGallery(ChildModel child)
        {
            if (IsTapped) return;

            IsTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ChildGalleryPage(child.child_id, child.child_name));
            IsTapped = false;
        }

        public async void OnGetChildGallery(string childId)
        {
            try
            {
                IsBusy = true;
                string url = "api/parent_panel/get_child_media_gallery";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = childId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GalleryListResult>(json);
                if (oResult.response_status == "200")
                {
                    lstChildGallery = new ObservableCollection<GalleryModel>();
                    foreach (var oGallery in oResult.data)
                    {
                        lstChildGallery.Add(oGallery);
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

        public async void OnOpenChildReports(ChildModel child)
        {
            if (IsTapped) return;

            IsTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ChildReportsPage(child.child_id, child.child_name));
            IsTapped = false;
        }

        public async void OnNewDayCare(ChildModel child)
        {
            if (IsTapped) return;

            IsTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new RequestDayCarePage(child.child_id, child.child_name));
            IsTapped = false;
        }

        public async void OnRequestDayCare()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (selected_daycare == null) { await Utility.ShowNotification("", AppResources.msgSelectDayCare); IsTapped = false; return; }

                IsBusy = true;
                string url = "api/parent_panel/add_daycare_join_request";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = ChildData.child_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "daycare_id", ParameterValue = selected_daycare.daycare_id });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    await Utility.ShowNotification("", AppResources.msgRequestSent);
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

        public async void OnOpenFile(GalleryModel galleryFile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }

            if (galleryFile.is_image == "0") await Application.Current.MainPage.Navigation.PushAsync(new VideoPlayerPage(galleryFile.file));
            else await Application.Current.MainPage.Navigation.PushAsync(new ImageViewerPage(galleryFile.file));

            IsTapped = false;
        }

        // Teacher Actions //
        public async void OnOpenChildProfile(string childId)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(childId))
            {
                await Utility.ShowNotification("", AppResources.msgMissedChildId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new ChildProfilePage(childId));
            IsTapped = false;
        }

        public async void OnContactParent(ChildModel child)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(child.parent_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedParentId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(child.parent_id, child.parent_name));
            IsTapped = false;
        }

        public async void OnCreateReport(ChildModel child)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(child.daycare_id) || string.IsNullOrEmpty(child.child_id))
            {
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new AddReportPage(child.daycare_id, child.child_id, child.child_name, child.child_face));
            IsTapped = false;
        }

        public async void OnParentNotifications(string parentId)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(parentId))
            {
                await Utility.ShowNotification("", AppResources.msgMissedParentId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new NotificationPage(parentId));
            IsTapped = false;
        }

        public async void OnUploadGallery(string childId)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(childId))
            {
                await Utility.ShowNotification("", AppResources.msgMissedChildId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new AddGalleryPage(childId));
            IsTapped = false;
        }

        public void OnCallParent(string parentMobile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (!string.IsNullOrEmpty(parentMobile)) Device.OpenUri(new Uri("tel:" + parentMobile));
            IsTapped = false;
        }
        #endregion

    }
}
