using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kangaroo.ViewModels
{
    public class AddReportViewModel : BaseViewModel
    {

        #region Declarations
        private string _daycare_id;
        private string _report_day;
        private string _child_id;
        private string _child_name;
        private string _child_face;
        private string _general_note;
        private string _child_progress;

        private List<ActivityTypeModel> _lstActivityTypes;
        private List<NutritionTypeModel> _lstNutritionTypes;
        private ObservableCollection<ActivityModel> _lstActivities;
        private ObservableCollection<ImageModel> _lstReportImages;

        private ActivityTypeModel _selected_activity_type;
        private TimeSpan _selected_time;
        private NutritionTypeModel _selected_nutrition_type;
        private string _desc;

        private bool _is_time_visible;
        private bool _is_nutrition_visible;

        private int _current_view_index;
        private string _current_view_title;
        private ContentView _current_view;
        #endregion

        #region Properties
        public string daycare_id
        {
            get { return _daycare_id; }
            set { SetProperty(ref _daycare_id, value); }
        }

        public string report_day
        {
            get { return _report_day; }
            set { SetProperty(ref _report_day, value); }
        }

        public string child_id
        {
            get { return _child_id; }
            set { SetProperty(ref _child_id, value); }
        }

        public string child_name
        {
            get { return _child_name; }
            set { SetProperty(ref _child_name, value); }
        }

        public string child_face
        {
            get { return _child_face; }
            set { SetProperty(ref _child_face, value); }
        }

        public string general_note
        {
            get { return _general_note; }
            set { SetProperty(ref _general_note, value); }
        }

        public string child_progress
        {
            get { return _child_progress; }
            set { SetProperty(ref _child_progress, value); }
        }

        public List<ActivityTypeModel> lstActivityTypes
        {
            get { return _lstActivityTypes; }
            set { SetProperty(ref _lstActivityTypes, value); }
        }

        public List<NutritionTypeModel> lstNutritionTypes
        {
            get { return _lstNutritionTypes; }
            set { SetProperty(ref _lstNutritionTypes, value); }
        }

        public ObservableCollection<ActivityModel> lstActivities
        {
            get { return _lstActivities; }
            set { SetProperty(ref _lstActivities, value); }
        }

        public ObservableCollection<ImageModel> lstReportImages
        {
            get { return _lstReportImages; }
            set { SetProperty(ref _lstReportImages, value); }
        }

        public ICommand PreviousCommand { get; }

        public ICommand NextCommand { get; }

        public ICommand AddActivityCommand { get; }

        public ICommand BackToActivityCommand { get; }

        public ICommand ImportActivityCommand { get; }

        public ICommand DeleteActivityCommand { get; }

        public ICommand AddReportCommand { get; }

        //**************************************************//
        public ICommand TakePhotoCommand { get; private set; }

        public ICommand PickPhotoCommand { get; private set; }

        public ICommand PickVideoCommand { get; private set; }

        public ICommand RemovePhotoCommand { get; private set; }

        public ICommand OpenFileCommand { get; }
        //**************************************************//

        public ActivityTypeModel selected_activity_type
        {
            get { return _selected_activity_type; }
            set
            {
                SetProperty(ref _selected_activity_type, value);

                if (value != null)
                {
                    switch (value.activity_id)
                    {
                        case "1": // Pathroom
                        case "4": // Trip
                        case "3": // Medication
                            is_time_visible = true;
                            is_nutrition_visible = false;
                            break;
                        case "2": // Nutrition
                            is_time_visible = true;
                            is_nutrition_visible = true;
                            break;
                        case "5": // Supply Needs
                            is_time_visible = false;
                            is_nutrition_visible = false;
                            break;
                    }
                }
            }
        }

        public TimeSpan selected_time
        {
            get { return _selected_time; }
            set { SetProperty(ref _selected_time, value); }
        }

        public NutritionTypeModel selected_nutrition_type
        {
            get { return _selected_nutrition_type; }
            set { SetProperty(ref _selected_nutrition_type, value); }
        }

        public string desc
        {
            get { return _desc; }
            set { SetProperty(ref _desc, value); }
        }

        public bool is_time_visible
        {
            get { return _is_time_visible; }
            set { SetProperty(ref _is_time_visible, value); }
        }

        public bool is_nutrition_visible
        {
            get { return _is_nutrition_visible; }
            set { SetProperty(ref _is_nutrition_visible, value); }
        }

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
        public AddReportViewModel(INavigation navigation)
        {
            Navigation = navigation;

            general_note = "";

            lstActivityTypes = new List<ActivityTypeModel>();
            lstNutritionTypes = new List<NutritionTypeModel>();
            lstActivities = new ObservableCollection<ActivityModel>();
            lstReportImages = new ObservableCollection<ImageModel>();

            PreviousCommand = new Command(OnPreviousView);
            NextCommand = new Command(OnNextView);
            AddActivityCommand = new Command(onAddActivity);
            BackToActivityCommand = new Command(OnBackToActivity);
            ImportActivityCommand = new Command(OnImportActivity);
            DeleteActivityCommand = new Command<int>(OnDeleteActivity);
            AddReportCommand = new Command(OnAddReport);

            TakePhotoCommand = new Command(OnTakePhoto);
            PickPhotoCommand = new Command(OnPickPhoto);
            PickVideoCommand = new Command(OnPickVideo);
            RemovePhotoCommand = new Command<int>(OnRemovePhoto);
            OpenFileCommand = new Command<ImageModel>(OnOpenFile);

            current_view_index = 0;
            current_view_title = AppResources.lbDailyReportActivity;
            current_view = new ActivityView();

            OnGetNutritionTypes();
            OnGetActivityTypes();
        }

        public async void OnGetActivityTypes()
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
                if (oResult.response_status == "200")
                {
                    lstActivityTypes = oResult.data;

                    lstActivities = new ObservableCollection<ActivityModel>();
                    foreach (var oActivityType in oResult.data)
                    {
                        var oActivityModel = new ActivityModel();

                        oActivityModel.activity_type_id = oActivityType.activity_id;
                        oActivityModel.activity_type_name = oActivityType.activity_type;
                        oActivityModel.order = 0;

                        if (oActivityType.activity_id == "2") oActivityModel.is_nutrition_header = true;
                        else if (oActivityType.activity_id == "5") oActivityModel.is_supply_header = true;
                        else oActivityModel.is_default_header = true;

                        lstActivities.Add(oActivityModel);
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

        public async void OnGetNutritionTypes()
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/get_daily_reports_nutrition_types";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    IsBusy = false;
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<NutritionTypeListResult>(json);
                if (oResult.response_status == "200") lstNutritionTypes = oResult.data;
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

        private void OnPreviousView()
        {
            try
            {
                switch (current_view_index)
                {
                    case 1:
                        current_view = new ActivityView();
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

        private async void OnNextView()
        {
            try
            {
                switch (current_view_index)
                {
                    case 0:
                        if (string.IsNullOrEmpty(general_note)) { await Utility.ShowNotification("", AppResources.msgReqGeneralNotes); return; }

                        current_view = new MediaView();
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

        private void onAddActivity()
        {
            try
            {
                is_time_visible = false;
                is_nutrition_visible = false;
                current_view = new AddActivityView();
                current_view_title = AppResources.lbAddActivity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void OnBackToActivity()
        {
            try
            {
                // Reset Current Activity
                is_time_visible = false;
                is_nutrition_visible = false;
                selected_activity_type = null;
                selected_time = TimeSpan.Zero;
                selected_nutrition_type = null;
                desc = "";

                // Set Current View
                current_view = new ActivityView();
                current_view_title = AppResources.lbDailyReportActivity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void OnImportActivity()
        {
            try
            {
                IsBusy = true;

                if (selected_activity_type == null) { await Utility.ShowNotification("", AppResources.msgReqActivityType); return; }

                // To Add New Activity here
                var oActivity = new ActivityModel();
                oActivity.daycare_id = daycare_id;
                oActivity.activity_type_id = selected_activity_type.activity_id;
                oActivity.serial = Utility.GetInteger(lstActivities.Max(a => a.serial)) + 1;
                oActivity.order = Utility.GetInteger(lstActivities.Where(a => a.activity_type_id == selected_activity_type.activity_id).Max(a => a.order)) + 1;
                oActivity.activity_desc = desc;

                switch (selected_activity_type.activity_id)
                {
                    case "1": // Pathroom
                        if (selected_time == TimeSpan.Zero) { await Utility.ShowNotification("", AppResources.msgReqTime); return; }

                        oActivity.activity_time = selected_time.ToString(@"hh\:mm");
                        oActivity.is_default_item = true;
                        break;
                    case "2": // Nutrition
                        if (selected_time == TimeSpan.Zero) { await Utility.ShowNotification("", AppResources.msgReqTime); return; }
                        if (selected_nutrition_type == null) { await Utility.ShowNotification("", AppResources.msgReqNutritionType); return; }

                        oActivity.activity_time = selected_time.ToString(@"hh\:mm");
                        oActivity.nutrition_type_id = selected_nutrition_type.nutrition_id;
                        oActivity.nutrition_type_name = lstNutritionTypes.Where(a => a.nutrition_id == selected_nutrition_type.nutrition_id).Select(a => a.nutrition_type).FirstOrDefault();
                        oActivity.is_nutrition_item = true;
                        break;
                    case "3": // Medication
                        if (selected_time == TimeSpan.Zero) { await Utility.ShowNotification("", AppResources.msgReqTime); return; }
                        if (string.IsNullOrEmpty(desc)) { await Utility.ShowNotification("", AppResources.msgReqDesc); return; }

                        oActivity.activity_time = selected_time.ToString(@"hh\:mm");
                        oActivity.is_default_item = true;
                        break;
                    case "4": // Trip
                        if (selected_time == TimeSpan.Zero) { await Utility.ShowNotification("", AppResources.msgReqTime); return; }
                        if (string.IsNullOrEmpty(desc)) { await Utility.ShowNotification("", AppResources.msgReqDesc); return; }

                        oActivity.activity_time = selected_time.ToString(@"hh\:mm");
                        oActivity.is_default_item = true;
                        break;
                    case "5": // Supply Needs
                        if (string.IsNullOrEmpty(desc)) { await Utility.ShowNotification("", AppResources.msgReqDesc); return; }

                        oActivity.is_supply_item = true;
                        break;
                }
                lstActivities.Add(oActivity);
                lstActivities = new ObservableCollection<ActivityModel>(lstActivities.OrderBy(a => a.activity_type_id).ThenBy(a => a.order).ToList());

                // Reset Current Activity
                is_time_visible = false;
                is_nutrition_visible = false;
                selected_activity_type = null;
                selected_time = TimeSpan.Zero;
                selected_nutrition_type = null;
                desc = "";

                // Set Current View
                current_view = new ActivityView();
                current_view_title = AppResources.lbDailyReportActivity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnDeleteActivity(int serial)
        {
            try
            {
                var oActivity = lstActivities.Where(a => a.serial == serial).FirstOrDefault();
                lstActivities.Remove(oActivity);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void OnTakePhoto()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                // Ref: https://github.com/jamesmontemagno/MediaPlugin
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to take photo", "OK");
                    return;
                }

                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                IsBusy = true;
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Kangaroo",
                    Name = "Item_Pic.jpg",
                    //PhotoSize = PhotoSize.Full,
                    //CompressionQuality = 30,
                    PhotoSize = PhotoSize.Small,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                {
                    IsBusy = false;
                    return;
                }

                var oImageData = new ImageModel();
                oImageData.media_type = "Photo";
                oImageData.serial = lstReportImages.Count + 1;
                oImageData.image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream()));
                oImageData.file_path = Utility.GetFileName(file.Path);
                oImageData.full_path = file.Path;

                lstReportImages.Add(oImageData);
                IsBusy = false;
                file.Dispose();
            }
            catch (MediaPermissionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Permission required", ex.Message, "OK");
                Log.Report(ex);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error - Take", ex.Message, "OK");
                Log.Report(ex);
            }
            finally
            {
                IsTapped = false;
                IsBusy = false;
            }
        }

        private async void OnPickPhoto()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                // Ref: https://github.com/jamesmontemagno/MediaPlugin
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to pick photo", "OK");
                    return;
                }

                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Photo", ":( Pick photos not available.", "OK");
                    return;
                }

                IsBusy = true;

                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Custom, CustomPhotoSize = 35 });
                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { CompressionQuality = 70  });
                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Full, CompressionQuality = 30 });
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Small });

                if (file == null)
                {
                    IsBusy = false;
                    return;
                }

                var oImageData = new ImageModel();
                oImageData.media_type = "Photo";
                oImageData.serial = lstReportImages.Count + 1;
                oImageData.image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream()));
                oImageData.file_path = Utility.GetFileName(file.Path);
                oImageData.full_path = file.Path;

                lstReportImages.Add(oImageData);
                IsBusy = false;
                file.Dispose();
            }
            catch (MediaPermissionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Permission required", ex.Message, "OK");
                Log.Report(ex);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error - Pick", ex.Message, "OK");
                Log.Report(ex);
            }
            finally
            {
                IsTapped = false;
                IsBusy = false;
            }
        }

        private async void OnPickVideo()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                // Ref: https://github.com/jamesmontemagno/MediaPlugin
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to pick video", "OK");
                    return;
                }

                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickVideoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Video", ":( Pick videos not available.", "OK");
                    return;
                }

                IsBusy = true;

                var file = await CrossMedia.Current.PickVideoAsync();

                if (file == null)
                {
                    IsBusy = false;
                    return;
                }

                var oImageData = new ImageModel();
                oImageData.media_type = "Video";
                oImageData.serial = lstReportImages.Count + 1;
                oImageData.video_stream = file.GetStream();
                oImageData.file_path = Path.GetFileName(file.Path);
                oImageData.full_path = file.Path;

                lstReportImages.Add(oImageData);
                IsBusy = false;
                file.Dispose();
            }
            catch (MediaPermissionException ex)
            {
                await App.Current.MainPage.DisplayAlert("Permission required", ex.Message, "OK");
                Log.Report(ex);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error - Pick", ex.Message, "OK");
                Log.Report(ex);
            }
            finally
            {
                IsTapped = false;
                IsBusy = false;
            }
        }

        private void OnRemovePhoto(int serial)
        {
            if (IsTapped) return;
            IsTapped = true;

            var oImage = lstReportImages.Where(a => a.serial == serial).FirstOrDefault();
            lstReportImages.Remove(oImage);
            IsTapped = false;
        }

        public async void OnOpenFile(ImageModel selectedFile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }

            if (selectedFile.media_type == "Video") await Application.Current.MainPage.Navigation.PushAsync(new VideoPlayerPage(selectedFile.full_path));
            else
            {
                //var Base64Stream = System.Convert.FromBase64String(selectedFile.image_content);
                //var imageSource = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                //await Application.Current.MainPage.Navigation.PushAsync(new ImageViewerPage(imageSource));

                await Application.Current.MainPage.Navigation.PushAsync(new ImageViewerPage(selectedFile.full_path));
            }

            IsTapped = false;
        }

        private async void OnAddReport()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/teacher_panel/add_daily_report";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "report_day", ParameterValue = report_day });
                lstParamters.Add(new ApiParameters() { ParameterName = "daycare_id", ParameterValue = daycare_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = child_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "teacher_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "general_note", ParameterValue = general_note });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_progress", ParameterValue = child_progress });

                var lstReportActivities = lstActivities.Where(a => a.order != 0).ToList();
                var activites = JsonConvert.SerializeObject(lstReportActivities);
                lstParamters.Add(new ApiParameters() { ParameterName = "activities", ParameterValue = activites });

                string json = await Utility.CallWebApiPost(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    string daily_report_id = oResult.data.ToString();
                    await OnAddReportImages(daily_report_id);
                    await OnAddReportVideos(daily_report_id);
                    await Utility.ShowNotification("", AppResources.msgReportAddedSuccessfully);
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

        private async Task<bool> OnAddReportImages(string reportId)
        {
            try
            {
                string url = "api/teacher_panel/add_daily_report_images";
                List<ApiParameters> lstParameters;
                List<ImageModel> lstImageToUpload;

                var lstImages = lstReportImages.Where(a => a.media_type == "Photo").ToList();
                foreach (var image in lstImages)
                {
                    var oImage = new ImageModel() { image_content = image.image_content, extension = image.GetExtension() };
                    lstImageToUpload = new List<ImageModel>() { oImage };
                    var images = JsonConvert.SerializeObject(lstImageToUpload);

                    lstParameters = new List<ApiParameters>();
                    lstParameters.Add(new ApiParameters() { ParameterName = "daily_report_id", ParameterValue = reportId });
                    lstParameters.Add(new ApiParameters() { ParameterName = "images", ParameterValue = images });
                    var result = await Utility.UploadFile(lstParameters, url);
                    if (result.response_status != "200") return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        private async Task<bool> OnAddReportVideos(string reportId)
        {
            try
            {
                string url = "api/teacher_panel/add_daily_report_videos";
                List<ApiParameters> lstParameters;

                var lstVideos = lstReportImages.Where(a => a.media_type == "Video").ToList();
                foreach (var video in lstVideos)
                {
                    lstParameters = new List<ApiParameters>();
                    lstParameters.Add(new ApiParameters() { ParameterName = "daily_report_id", ParameterValue = reportId });
                    var result = await Utility.UploadVideo(lstParameters, video.video_stream, "video", video.file_path, url);
                    if (result.response_status != "200") return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        #endregion

    }
}
