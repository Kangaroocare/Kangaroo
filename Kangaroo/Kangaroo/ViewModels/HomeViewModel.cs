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

    public class HomeViewModel : BaseViewModel
    {

        #region Declarations
        private bool isTapped = false;

        private int _current_page_search_projects;
        private bool _load_more_search_projects;

        private bool _is_featured_projects_visible;
        private int _main_category_list_height;

        private string _search_keyword;
        private FilterModel _filter_model;

        private View _CurrentView;
        private ObservableCollection<DayCareModel> _lstDayCares;
        private ObservableCollection<DayCareModel> _lstFilteredDayCares;
        private ObservableCollection<NotificationModel> _lstNotifications;
        private ObservableCollection<MessageModel> _lstMessageList;
        private ObservableCollection<ClassModel> _lstClasses;
        private ObservableCollection<ClassChildrenModel> _lstClassChildren;
        private ObservableCollection<ClassChildrenModel> _lstMyChildren;

        private ClassModel _selected_class;

        private Color _FirstViewColor, _SecondViewColor, _ThirdViewColor, _FourthViewColor;
        private bool _IsFirstViewSelected, _IsSecondViewSelected, _IsThirdViewSelected, _IsFourthViewSelected;
        private string _FirstViewText, _SecondViewText, _ThirdViewText, _FourthViewText;
        private bool _IsUnRegistered, _IsTeacher, _IsParent;
        #endregion

        #region Properties
        public int current_page_search_projects
        {
            get { return _current_page_search_projects; }
            set { SetProperty(ref _current_page_search_projects, value); }
        }

        public bool load_more_search_projects
        {
            get { return _load_more_search_projects; }
            set { SetProperty(ref _load_more_search_projects, value); }
        }

        public bool is_featured_projects_visible
        {
            get { return _is_featured_projects_visible; }
            set { SetProperty(ref _is_featured_projects_visible, value); }
        }

        public int main_category_list_height
        {
            get { return _main_category_list_height; }
            set { SetProperty(ref _main_category_list_height, value); }
        }

        public Color FirstViewColor
        {
            get { return _FirstViewColor; }
            set { SetProperty(ref _FirstViewColor, value); }
        }

        public Color SecondViewColor
        {
            get { return _SecondViewColor; }
            set { SetProperty(ref _SecondViewColor, value); }
        }

        public Color ThirdViewColor
        {
            get { return _ThirdViewColor; }
            set { SetProperty(ref _ThirdViewColor, value); }
        }

        public Color FourthViewColor
        {
            get { return _FourthViewColor; }
            set { SetProperty(ref _FourthViewColor, value); }
        }

        public bool IsFirstViewSelected
        {
            get { return _IsFirstViewSelected; }
            set { SetProperty(ref _IsFirstViewSelected, value); }
        }

        public bool IsSecondViewSelected
        {
            get { return _IsSecondViewSelected; }
            set { SetProperty(ref _IsSecondViewSelected, value); }
        }

        public bool IsThirdViewSelected
        {
            get { return _IsThirdViewSelected; }
            set { SetProperty(ref _IsThirdViewSelected, value); }
        }

        public bool IsFourthViewSelected
        {
            get { return _IsFourthViewSelected; }
            set { SetProperty(ref _IsFourthViewSelected, value); }
        }

        public string FirstViewText
        {
            get { return _FirstViewText; }
            set { SetProperty(ref _FirstViewText, value); }
        }

        public string SecondViewText
        {
            get { return _SecondViewText; }
            set { SetProperty(ref _SecondViewText, value); }
        }

        public string ThirdViewText
        {
            get { return _ThirdViewText; }
            set { SetProperty(ref _ThirdViewText, value); }
        }

        public string FourthViewText
        {
            get { return _FourthViewText; }
            set { SetProperty(ref _FourthViewText, value); }
        }

        public bool IsUnRegistered
        {
            get { return _IsUnRegistered; }
            set { SetProperty(ref _IsUnRegistered, value); }
        }

        public bool IsTeacher
        {
            get { return _IsTeacher; }
            set { SetProperty(ref _IsTeacher, value); }
        }

        public bool IsParent
        {
            get { return _IsParent; }
            set { SetProperty(ref _IsParent, value); }
        }

        public string search_keyword
        {
            get { return _search_keyword; }
            set
            {
                SetProperty(ref _search_keyword, value);
                if (!string.IsNullOrEmpty(value)) lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstDayCares.Where(a => a.daycare_name.Contains(value)));
                else if (lstDayCares != null) lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstDayCares);
            }
        }

        public FilterModel filter_model
        {
            get { return _filter_model; }
            set { SetProperty(ref _filter_model, value); }
        }

        public View CurrentView
        {
            get { return _CurrentView; }
            set
            {
                SetProperty(ref _CurrentView, value);
                FirstViewColor = SecondViewColor = ThirdViewColor = FourthViewColor = (Color)Application.Current.Resources["TextColor"];
                IsFirstViewSelected = IsSecondViewSelected = IsThirdViewSelected = IsFourthViewSelected = false;
                IsUnRegistered = IsTeacher = IsParent = false;

                FirstViewText = AppResources.lbMain;
                FourthViewText = AppResources.lbMore;

                if (Settings.UserData == null) // UnRegistered
                {
                    IsUnRegistered = true;
                    SecondViewText = AppResources.lbDayCares;
                    ThirdViewText = AppResources.lbRegister;
                }
                else if (Settings.UserData.user_type == "3") // Teacher
                {
                    IsTeacher = true;
                    SecondViewText = AppResources.lbClasses;
                    ThirdViewText = AppResources.lbAttendance;
                }
                else if (Settings.UserData.user_type == "4") // Parent
                {
                    IsParent = true;
                    SecondViewText = AppResources.lbMyChildren;
                    ThirdViewText = AppResources.lbDayCares;
                }

                switch (Settings.HomeTabIndex)
                {
                    case 0:
                        IsFirstViewSelected = true;
                        FirstViewColor = (Color)Application.Current.Resources["BrandColor"];
                        break;
                    case 1:
                        IsSecondViewSelected = true;
                        SecondViewColor = (Color)Application.Current.Resources["BrandColor"];
                        break;
                    case 2:
                        IsThirdViewSelected = true;
                        ThirdViewColor = (Color)Application.Current.Resources["BrandColor"];
                        break;
                    case 3:
                        IsFourthViewSelected = true;
                        FourthViewColor = (Color)Application.Current.Resources["BrandColor"];
                        break;
                }
            }
        }

        public ObservableCollection<DayCareModel> lstDayCares
        {
            get { return _lstDayCares; }
            set { SetProperty(ref _lstDayCares, value); }
        }

        public ObservableCollection<DayCareModel> lstFilteredDayCares
        {
            get { return _lstFilteredDayCares; }
            set { SetProperty(ref _lstFilteredDayCares, value); }
        }

        public ObservableCollection<NotificationModel> lstNotifications
        {
            get { return _lstNotifications; }
            set { SetProperty(ref _lstNotifications, value); }
        }

        public ObservableCollection<MessageModel> lstMessageList
        {
            get { return _lstMessageList; }
            set { SetProperty(ref _lstMessageList, value); }
        }

        public ObservableCollection<ClassModel> lstClasses
        {
            get { return _lstClasses; }
            set { SetProperty(ref _lstClasses, value); }
        }

        public ObservableCollection<ClassChildrenModel> lstClassChildren
        {
            get { return _lstClassChildren; }
            set { SetProperty(ref _lstClassChildren, value); }
        }

        public ObservableCollection<ClassChildrenModel> lstMyChildren
        {
            get { return _lstMyChildren; }
            set { SetProperty(ref _lstMyChildren, value); }
        }

        public ClassModel selected_class
        {
            get { return _selected_class; }
            set
            {
                SetProperty(ref _selected_class, value);
                if (selected_class != null)
                {
                    lstClassChildren = new ObservableCollection<ClassChildrenModel>();
                    OnGetClassChildren();
                }
            }
        }

        public ICommand FirstViewCommand { get; private set; }

        public ICommand SecondViewCommand { get; private set; }

        public ICommand ThirdViewCommand { get; private set; }

        public ICommand FourthViewCommand { get; private set; }

        public ICommand AboutKangarooCommand { get; private set; }

        public ICommand FavoriteCommand { get; private set; }

        public ICommand OpenDayCareCommand { get; private set; }

        public ICommand ParentChildProfileCommand { get; private set; }

        public ICommand ContactDayCareCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }

        public ICommand LoadMoreSearchCommand { get; private set; }

        public ICommand DialParentCommand { get; private set; }

        public ICommand ContactParentCommand { get; private set; }

        public ICommand DialTeacherCommand { get; private set; }

        public ICommand ContactTeacherCommand { get; private set; }

        public ICommand TeacherChildProfileCommand { get; private set; }

        public ICommand ContactSenderCommand { get; private set; }

        public ICommand FilterDayCaresCommand { get; private set; }
        #endregion

        #region Functions
        public HomeViewModel(INavigation navigation)
        {
            Navigation = navigation;

            current_page_search_projects = 0;
            load_more_search_projects = true;
            is_featured_projects_visible = true;
            main_category_list_height = 0;

            FirstViewCommand = new Command(OnFirstView);
            SecondViewCommand = new Command(OnSecondView);
            ThirdViewCommand = new Command(OnThirdView);
            FourthViewCommand = new Command(OnFourthView);

            AboutKangarooCommand = new Command(OnAboutKangaroo);
            FavoriteCommand = new Command<string>(OnFavorite);
            OpenDayCareCommand = new Command<string>(OnOpenDayCare);
            ParentChildProfileCommand = new Command<string>(OnOpenParentChildProfile);
            ContactDayCareCommand = new Command<DayCareModel>(OnContactDayCare);
            DialParentCommand = new Command<string>(OnDialParent);
            ContactParentCommand = new Command<ClassChildrenModel>(OnContactParent);
            DialTeacherCommand = new Command<string>(OnDialTeacher);
            ContactTeacherCommand = new Command<ClassChildrenModel>(OnContactTeacher);
            TeacherChildProfileCommand = new Command<string>(OnOpenTeacherChildProfile);
            ContactSenderCommand = new Command<NotificationModel>(OnContactSender);
            FilterDayCaresCommand = new Command(OnFilterDayCares);

            lstDayCares = new ObservableCollection<DayCareModel>();
            lstFilteredDayCares = new ObservableCollection<DayCareModel>();
            lstNotifications = new ObservableCollection<NotificationModel>();
            lstMessageList = new ObservableCollection<MessageModel>();
            lstClasses = new ObservableCollection<ClassModel>();
            lstClassChildren = new ObservableCollection<ClassChildrenModel>();
            lstMyChildren = new ObservableCollection<ClassChildrenModel>();

            search_keyword = "";

            Settings.HomeTabIndex = 0;
            if (Settings.UserData == null) CurrentView = new DayCaresView();
            else CurrentView = new NotificationView();
        }

        public void OnFirstView()
        {
            Settings.HomeTabIndex = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Settings.UserData == null) // UnRegistered
                {
                    CurrentView = new DayCaresView();
                    lstDayCares = new ObservableCollection<DayCareModel>();
                    OnGetDayCares();
                }
                else // Teachers & Parents
                {
                    CurrentView = new NotificationView();
                    lstNotifications = new ObservableCollection<NotificationModel>();
                    OnGetNotifications();
                }
                Task.Delay(2000);
            });
        }

        public void OnSecondView()
        {
            Settings.HomeTabIndex = 1;
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Settings.UserData == null) // UnRegistered
                {
                    CurrentView = new MapView(lstDayCares.ToList());
                }
                else if (Settings.UserData.user_type == "3") // Teacher
                {
                    CurrentView = new ClassesView();
                    lstClasses = new ObservableCollection<ClassModel>();
                    await OnGetClasses();
                }
                else if (Settings.UserData.user_type == "4") // Parent
                {
                    CurrentView = new MyChildrenView();
                    lstMyChildren = new ObservableCollection<ClassChildrenModel>();
                    OnGetMyChildren();
                }
                await Task.Delay(2000);
            });
        }

        public async void OnThirdView()
        {
            if (Settings.UserData == null)
            {
                if (isTapped) return;
                isTapped = true;
                Settings.HomeTabIndex = 0;
                //await Application.Current.MainPage.Navigation.PushAsync(new CreateUserPage());
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                isTapped = false;
            }
            else
            {
                Settings.HomeTabIndex = 2;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (Settings.UserData.user_type == "3") // Teacher
                    {
                        CurrentView = new CheckInView();
                        if (selected_class == null)
                        {
                            if (lstClasses.Count == 0) await OnGetClasses();
                            if (selected_class == null) selected_class = lstClasses[0];
                        }
                    }
                    else if (Settings.UserData.user_type == "4") // Parent
                    {
                        CurrentView = new DayCaresView();
                        lstDayCares = new ObservableCollection<DayCareModel>();
                        OnGetDayCares();
                    }
                    await Task.Delay(2000);
                });
            }
        }

        public void OnFourthView()
        {
            Settings.HomeTabIndex = 3;
            Device.BeginInvokeOnMainThread(() =>
            {
                CurrentView = new MenuView();
            });
        }

        private async void OnAboutKangaroo()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/pages/get_page_by_id/";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "page_id", ParameterValue = "2" });
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<PageResult>(json);
                if (oResult.response_status == "200" && oResult.data != null)
                {
                    var oPage = oResult.data;
                    string sDirection = (Settings.Language == "ar") ? "rtl" : "ltr";
                    string html = string.Format("<div dir='{0}' style='margin:10'>{1}</div> ", sDirection, oPage.page_content);
                    await Application.Current.MainPage.Navigation.PushAsync(new WebViewPage(html, WebViewPage.SourceType.HTML, oPage.page_title, ""));
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

        public async void OnFavorite(string daycareId)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/parent_panel/add_favorite";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "daycare_id", ParameterValue = daycareId });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    OnGetDayCares();
                }
                //else await Utility.ShowNotification("", oResult.response_message);
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

                    if (string.IsNullOrEmpty(search_keyword)) lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstDayCares);
                    else lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstDayCares.Where(a => a.daycare_name.Contains(search_keyword)));

                    if (filter_model != null)
                    {
                        if (filter_model.is_special_needs != "0") lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstFilteredDayCares.Where(a => a.is_special_needs == filter_model.is_special_needs));
                        if (filter_model.lstSelectedCities.Count > 0) lstFilteredDayCares = new ObservableCollection<DayCareModel>(lstFilteredDayCares.Where(a => filter_model.lstSelectedCities.Any(b => b.id == a.city_id)));
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

        public async void OnGetNotifications()
        {
            try
            {
                IsBusy = true;
                string url = "api/notifications/get_notifications_list";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });

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

        public async Task OnGetClasses()
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/my_classes";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ClassListResult>(json);
                if (oResult.response_status == "200")
                {
                    int index = 1;
                    foreach (var oClass in oResult.data)
                    {
                        if (index % 2 == 0) oClass.class_color = "#f5ebfa";
                        else oClass.class_color = "#ffffff";

                        lstClasses.Add(oClass);
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

        public async void OnGetClassChildren()
        {
            try
            {
                IsBusy = true;
                string url = "api/teacher_panel/class_children";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "class_id", ParameterValue = selected_class.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "care_date", ParameterValue = DateTime.Now.ToString("yyyy-MM-dd") });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ClassChildrenResult>(json);
                if (oResult.response_status == "200")
                {
                    lstClassChildren = new ObservableCollection<ClassChildrenModel>();
                    foreach (var oChildern in oResult.data)
                    {
                        lstClassChildren.Add(oChildern);
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

        public async void OnGetMyChildren()
        {
            try
            {
                IsBusy = true;
                string url = "api/parent_panel/my_children";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "parent_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "care_date", ParameterValue = DateTime.Now.ToString("yyyy-MM-dd") });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<ClassChildrenResult>(json);
                if (oResult.response_status == "200")
                {
                    lstMyChildren = new ObservableCollection<ClassChildrenModel>();
                    foreach (var oChildern in oResult.data)
                    {
                        lstMyChildren.Add(oChildern);
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

        public async void OnOpenDayCare(string dayCareId)
        {
            if (IsTapped) return;
            IsTapped = true;

            await Application.Current.MainPage.Navigation.PushAsync(new DayCareDetailsPage(dayCareId));
            IsTapped = false;
        }

        public async void OnOpenParentChildProfile(string childId)
        {
            if (IsTapped) return;

            IsTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new ParentChildProfilePage(childId));
            IsTapped = false;
        }

        public async void OnContactDayCare(DayCareModel daycare)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                IsTapped = false;
                return;
            }
            if (string.IsNullOrEmpty(daycare.school_admin_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedDayCareAdminId);
                IsTapped = false;
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(daycare.school_admin_id, daycare.daycare_name));
            IsTapped = false;
        }

        public void OnDialParent(string parentMobile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (!string.IsNullOrEmpty(parentMobile)) Device.OpenUri(new Uri("tel:" + parentMobile));
            IsTapped = false;
        }

        public async void OnContactParent(ClassChildrenModel classChild)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                return;
            }
            if (string.IsNullOrEmpty(classChild.parent_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedParentId);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(classChild.parent_id, classChild.parent_name));
            IsTapped = false;
        }

        public void OnDialTeacher(string teacherMobile)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (!string.IsNullOrEmpty(teacherMobile)) Device.OpenUri(new Uri("tel:" + teacherMobile));
            IsTapped = false;
        }

        public async void OnContactTeacher(ClassChildrenModel classChild)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                return;
            }
            if (string.IsNullOrEmpty(classChild.teacher_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedTeacherId);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(classChild.teacher_id, classChild.teacher_name));
            IsTapped = false;
        }

        public async void OnOpenTeacherChildProfile(string childId)
        {
            if (IsTapped) return;

            IsTapped = true;
            await Application.Current.MainPage.Navigation.PushAsync(new TeacherChildProfilePage(childId));
            IsTapped = false;
        }

        public async void OnContactSender(NotificationModel notification)
        {
            if (IsTapped) return;
            IsTapped = true;

            if (Settings.UserData == null || string.IsNullOrEmpty(Settings.UserId))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                return;
            }
            if (string.IsNullOrEmpty(notification.sender_id))
            {
                await Utility.ShowNotification("", AppResources.msgMissedSenderId);
                return;
            }

            await OnResetNotificationsCounter(notification.sender_id);
            await Application.Current.MainPage.Navigation.PushAsync(new MessageDetailsPage(notification.sender_id, notification.sender_name));
            IsTapped = false;
        }

        public async Task OnResetNotificationsCounter(string senderId)
        {
            try
            {
                IsBusy = true;
                string url = "api/notifications/reset_notifications_counter";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "user_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "sender_id", ParameterValue = senderId });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    // Do Nothing
                }
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

        public async void OnFilterDayCares()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                IsBusy = true;
                string url = "api/settings/get_cities";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                lstParamters.Add(new ApiParameters() { ParameterName = "country_id", ParameterValue = "2" });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<CityListResult>(json);
                if (oResult.response_status == "200")
                {
                    var lstData = new ObservableCollection<CityModel>(oResult.data);
                    var oFilterDayCares = new FilterDayCares(lstData);
                    await Application.Current.MainPage.Navigation.PushAsync(oFilterDayCares);
                    oFilterDayCares.OnApplyFilter += OnApplyDayCaresFilter;
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

        private async void OnApplyDayCaresFilter(object sender, FilterModel daycaresFilter)
        {
            try
            {
                filter_model = daycaresFilter;
            }
            catch (Exception ex)
            {
                await Utility.ShowNotification("", ex.Message);
            }
        }
        #endregion

    }
}