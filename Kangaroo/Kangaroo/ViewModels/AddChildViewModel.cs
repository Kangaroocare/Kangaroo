using Kangaroo.Controls;
using Kangaroo.Helpers;
using Kangaroo.Models;
using Kangaroo.Resources;
using Kangaroo.Views;
using Kangaroo.Views.ChildViews;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Kangaroo.ViewModels
{

    public class AddChildViewModel : BaseViewModel
    {

        #region Declarations
        private ChildrenModel _child_data;

        private string fileIndex;
        private bool _child_face_uploading;
        private bool _father_id_uploading;
        private bool _mother_id_uploading;

        private int _home_relations_height;

        private ImageModel _child_face;
        private ImageModel _father_id;
        private ImageModel _mother_id;

        private List<ListItemModel> _lstGender;
        private List<SpecialNeedModel> _lstSpecialNeeds;
        private List<ListItemModel> _lstTakePhoto;
        private List<RelationModel> _lstRelations;
        private ObservableCollection<HomeRelationModel> _lstHomeRelations;

        private ListItemModel _selected_gender;
        private SpecialNeedModel _selected_special_need;
        private ListItemModel _selected_take_photo;
        private RelationModel _selected_em_relation;

        private int _current_step;
        private int _current_view_index;
        private string _current_view_title;
        private ContentView _current_view;
        #endregion

        #region Properties
        public ChildrenModel child_data
        {
            get { return _child_data; }
            set { SetProperty(ref _child_data, value); }
        }

        public bool child_face_uploading
        {
            get { return _child_face_uploading; }
            set { SetProperty(ref _child_face_uploading, value); }
        }

        public bool mother_id_uploading
        {
            get { return _mother_id_uploading; }
            set { SetProperty(ref _mother_id_uploading, value); }
        }

        public bool father_id_uploading
        {
            get { return _father_id_uploading; }
            set { SetProperty(ref _father_id_uploading, value); }
        }

        public int home_relations_height
        {
            get { return _home_relations_height; }
            set { SetProperty(ref _home_relations_height, value); }
        }

        public ImageModel child_face
        {
            get { return _child_face; }
            set { SetProperty(ref _child_face, value); }
        }

        public ImageModel mother_id
        {
            get { return _mother_id; }
            set { SetProperty(ref _mother_id, value); }
        }

        public ImageModel father_id
        {
            get { return _father_id; }
            set { SetProperty(ref _father_id, value); }
        }

        public List<ListItemModel> lstGender
        {
            get { return _lstGender; }
            set { SetProperty(ref _lstGender, value); }
        }

        public List<SpecialNeedModel> lstSpecialNeeds
        {
            get { return _lstSpecialNeeds; }
            set { SetProperty(ref _lstSpecialNeeds, value); }
        }

        public List<ListItemModel> lstTakePhoto
        {
            get { return _lstTakePhoto; }
            set { SetProperty(ref _lstTakePhoto, value); }
        }

        public List<RelationModel> lstRelations
        {
            get { return _lstRelations; }
            set { SetProperty(ref _lstRelations, value); }
        }

        public ObservableCollection<HomeRelationModel> lstHomeRelations
        {
            get { return _lstHomeRelations; }
            set { SetProperty(ref _lstHomeRelations, value); }
        }

        public ListItemModel selected_gender
        {
            get { return _selected_gender; }
            set { SetProperty(ref _selected_gender, value); }
        }

        public SpecialNeedModel selected_special_need
        {
            get { return _selected_special_need; }
            set { SetProperty(ref _selected_special_need, value ?? _selected_special_need); }
        }

        public ListItemModel selected_take_photo
        {
            get { return _selected_take_photo; }
            set { SetProperty(ref _selected_take_photo, value); }
        }

        public RelationModel selected_em_relation
        {
            get { return _selected_em_relation; }
            set { SetProperty(ref _selected_em_relation, value ?? _selected_em_relation); }
        }

        public ICommand PreviousCommand { get; }

        public ICommand NextCommand { get; }

        public ICommand TakePicCommand { get; private set; }

        public ICommand AddPicCommand { get; private set; }

        public ICommand RemovePicCommand { get; private set; }

        public ICommand AddRelationCommand { get; private set; }

        public ICommand RemoveRelationCommand { get; private set; }

        public ICommand AddChildCommand { get; private set; }

        public int current_step
        {
            get { return _current_step; }
            set { SetProperty(ref _current_step, value); }
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
        public AddChildViewModel(INavigation navigation)
        {
            Navigation = navigation;

            child_data = new ChildrenModel();

            home_relations_height = 185;

            lstGender = new List<ListItemModel>();
            lstGender.Add(new ListItemModel() { id = "1", name = AppResources.lbMale });
            lstGender.Add(new ListItemModel() { id = "2", name = AppResources.lbFemale });
            selected_gender = lstGender[0];

            lstTakePhoto = new List<ListItemModel>();
            lstTakePhoto.Add(new ListItemModel() { id = "1", name = AppResources.lbYes });
            lstTakePhoto.Add(new ListItemModel() { id = "0", name = AppResources.lbNo });
            selected_take_photo = lstTakePhoto[0];

            lstSpecialNeeds = new List<SpecialNeedModel>();
            lstRelations = new List<RelationModel>();
            lstHomeRelations = new ObservableCollection<HomeRelationModel>();

            PreviousCommand = new Command(OnPreviousView);
            NextCommand = new Command(OnNextView);
            AddRelationCommand = new Command(OnAddRelation);
            RemoveRelationCommand = new Command<int>(OnRemoveRelation);
            AddChildCommand = new Command(OnAddChild);
            TakePicCommand = new Command<string>(OnTakePic);
            AddPicCommand = new Command<string>(OnAddPic);
            RemovePicCommand = new Command<string>(OnRemovePic);

            current_step = 1;
            current_view_index = 0;
            current_view_title = AppResources.lbChildData;
            current_view = new ChildView();

            OnGetSpecialNeeds();
            OnGetRelations();
        }

        private void OnPreviousView()
        {
            try
            {
                switch (current_view_index)
                {
                    case 1:
                        current_view = new ChildView();
                        current_view_title = AppResources.lbChildData;
                        break;
                    case 2:
                        current_view = new ContactView();
                        current_view_title = AppResources.lbContactData;
                        break;
                    case 3:
                        current_view = new ParentView();
                        current_view_title = AppResources.lbParentData;
                        break;
                }
                current_step -= 1;
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
                        if (string.IsNullOrEmpty(child_data.national_id)) { await Utility.ShowNotification("", AppResources.msgReqChildNationalId); return; }
                        if (string.IsNullOrEmpty(child_data.child_name)) { await Utility.ShowNotification("", AppResources.msgReqChildName); return; }
                        if (string.IsNullOrEmpty(child_data.date_of_birth)) { await Utility.ShowNotification("", AppResources.msgReqBirthDate); return; }
                        if (string.IsNullOrEmpty(child_data.address)) { await Utility.ShowNotification("", AppResources.msgReqChildAddress); return; }
                        if (string.IsNullOrEmpty(child_data.health_condition)) { await Utility.ShowNotification("", AppResources.msgReqHealthCondition); return; }
                        if (string.IsNullOrEmpty(child_data.what_to_know)) { await Utility.ShowNotification("", AppResources.msgReqWhatToKnow); return; }
                        if (string.IsNullOrEmpty(child_data.start_date)) { await Utility.ShowNotification("", AppResources.msgReqStartDate); return; }
                        //if (string.IsNullOrEmpty(child_data.end_date)) { await Utility.ShowNotification("", AppResources.msgReqEndDate); return; }

                        current_view = new ContactView();
                        current_view_title = AppResources.lbContactData;
                        break;
                    case 1:
                        foreach (var oHomeRelation in lstHomeRelations)
                        {
                            if (string.IsNullOrEmpty(oHomeRelation.contact_name)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationName); return; }
                            if (string.IsNullOrEmpty(oHomeRelation.contact_number)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationMobile); return; }
                            if (oHomeRelation.selected_relation == null) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationId); return; }
                            if (string.IsNullOrEmpty(oHomeRelation.national_id)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationNationalId); return; }
                        }
                        if (selected_em_relation == null) { await Utility.ShowNotification("", AppResources.msgReqEmergencyRelation); return; }
                        if (string.IsNullOrEmpty(child_data.em_contact_name)) { await Utility.ShowNotification("", AppResources.msgReqEmergencyName); return; }
                        if (string.IsNullOrEmpty(child_data.em_contact_mobile)) { await Utility.ShowNotification("", AppResources.msgReqEmergencyMobile); return; }

                        current_view = new ParentView();
                        current_view_title = AppResources.lbParentData;
                        break;
                    case 2:
                        if (string.IsNullOrEmpty(child_data.mother_name)) { await Utility.ShowNotification("", AppResources.msgReqMotherName); return; }
                        if (string.IsNullOrEmpty(child_data.mother_national_id)) { await Utility.ShowNotification("", AppResources.msgReqMotherNationalId); return; }
                        if (string.IsNullOrEmpty(child_data.mother_address)) { await Utility.ShowNotification("", AppResources.msgReqMotherAddress); return; }
                        if (string.IsNullOrEmpty(child_data.mother_mobile)) { await Utility.ShowNotification("", AppResources.msgReqMotherMobile); return; }
                        if (string.IsNullOrEmpty(child_data.father_name)) { await Utility.ShowNotification("", AppResources.msgReqFatherName); return; }
                        if (string.IsNullOrEmpty(child_data.father_national_id)) { await Utility.ShowNotification("", AppResources.msgReqFatherNationalId); return; }
                        if (string.IsNullOrEmpty(child_data.father_address)) { await Utility.ShowNotification("", AppResources.msgReqFatherAddress); return; }
                        if (string.IsNullOrEmpty(child_data.father_mobile)) { await Utility.ShowNotification("", AppResources.msgReqFatherMobile); return; }
                        if (string.IsNullOrEmpty(child_data.siblings_number)) { await Utility.ShowNotification("", AppResources.msgReqSiblingsNumber); return; }

                        current_view = new ImagesView();
                        current_view_title = AppResources.lbImages;
                        break;
                }
                current_step += 1;
                current_view_index += 1;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public async void OnGetRelations()
        {
            try
            {
                IsBusy = true;
                string url = "api/settings/get_relations";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<RelationListResult>(json);
                if (oResult.response_status == "200")
                {
                    lstRelations = oResult.data;

                    var oHomeRelation = new HomeRelationModel();
                    oHomeRelation.serial = 1;
                    oHomeRelation.lstRelations = oResult.data;

                    // Add logged parent data by default
                    oHomeRelation.contact_name = Settings.UserData.full_name;
                    oHomeRelation.contact_number = Settings.UserData.mobile;
                    oHomeRelation.national_id = Settings.UserData.national_id;
                    oHomeRelation.selected_relation = oResult.data.Where(a => a.id == Settings.UserData.child_relation_id).FirstOrDefault();
                    //

                    lstHomeRelations.Add(oHomeRelation);

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

        public async void OnGetSpecialNeeds()
        {
            try
            {
                IsBusy = true;
                string url = "api/settings/get_special_needs";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<SpecialNeedListResult>(json);
                if (oResult.response_status == "200")
                {
                    oResult.data.Insert(0, new SpecialNeedModel { id = "", special_need_name = "", special_need_desc = "" });
                    lstSpecialNeeds = oResult.data;
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

        private async void OnAddRelation()
        {
            try
            {
                foreach (var oRelation in lstHomeRelations)
                {
                    if (string.IsNullOrEmpty(oRelation.contact_name)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationName); return; }
                    if (string.IsNullOrEmpty(oRelation.contact_number)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationMobile); return; }
                    if (oRelation.selected_relation == null) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationId); return; }
                    if (string.IsNullOrEmpty(oRelation.national_id)) { await Utility.ShowNotification("", AppResources.msgReqHomeRelationNationalId); return; }
                }

                var oHomeRelation = new HomeRelationModel();
                oHomeRelation.serial = lstHomeRelations.Count + 1;
                oHomeRelation.lstRelations = lstRelations;
                lstHomeRelations.Add(oHomeRelation);
                home_relations_height += 180;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void OnRemoveRelation(int serial)
        {
            try
            {
                if (lstHomeRelations.Count == 1) { await Utility.ShowNotification("", AppResources.msgReqOneRelation); return; }

                var oHomeRelation = lstHomeRelations.Where(a => a.serial == serial).FirstOrDefault();
                if (oHomeRelation != null)
                {
                    lstHomeRelations.Remove(oHomeRelation);
                    home_relations_height -= 180;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private async void OnAddChild()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (child_face == null) { await Utility.ShowNotification("", AppResources.msgReqChildFace); return; }
                if (mother_id == null) { await Utility.ShowNotification("", AppResources.msgReqMotherId); return; }
                if (father_id == null) { await Utility.ShowNotification("", AppResources.msgReqFatherId); return; }

                IsBusy = true;
                string url = "api/parent_panel/add_child_profile";
                var lstParamters = new List<ApiParameters>();
                lstParamters.Add(new ApiParameters() { ParameterName = "parent_id", ParameterValue = Settings.UserId });
                lstParamters.Add(new ApiParameters() { ParameterName = "national_id", ParameterValue = child_data.national_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "child_name", ParameterValue = child_data.child_name });
                lstParamters.Add(new ApiParameters() { ParameterName = "date_of_birth", ParameterValue = child_data.date_of_birth });
                lstParamters.Add(new ApiParameters() { ParameterName = "gender", ParameterValue = selected_gender.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "address", ParameterValue = child_data.address });
                lstParamters.Add(new ApiParameters() { ParameterName = "mother_name", ParameterValue = child_data.mother_name });
                lstParamters.Add(new ApiParameters() { ParameterName = "mother_national_id", ParameterValue = child_data.mother_national_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "mother_address", ParameterValue = child_data.mother_address });
                lstParamters.Add(new ApiParameters() { ParameterName = "mother_mobile", ParameterValue = child_data.mother_mobile });
                lstParamters.Add(new ApiParameters() { ParameterName = "father_name", ParameterValue = child_data.father_name });
                lstParamters.Add(new ApiParameters() { ParameterName = "father_national_id", ParameterValue = child_data.father_national_id });
                lstParamters.Add(new ApiParameters() { ParameterName = "father_address", ParameterValue = child_data.father_address });
                lstParamters.Add(new ApiParameters() { ParameterName = "father_mobile", ParameterValue = child_data.father_mobile });
                lstParamters.Add(new ApiParameters() { ParameterName = "siblings_number", ParameterValue = child_data.siblings_number });
                lstParamters.Add(new ApiParameters() { ParameterName = "health_condition", ParameterValue = child_data.health_condition });
                if (selected_special_need != null) lstParamters.Add(new ApiParameters() { ParameterName = "special_needs_id", ParameterValue = selected_special_need.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "what_to_know", ParameterValue = child_data.what_to_know });
                lstParamters.Add(new ApiParameters() { ParameterName = "can_take_photo", ParameterValue = selected_take_photo.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "em_contact_relation", ParameterValue = selected_em_relation.id });
                lstParamters.Add(new ApiParameters() { ParameterName = "em_contact_name", ParameterValue = child_data.em_contact_name });
                lstParamters.Add(new ApiParameters() { ParameterName = "em_contact_mobile", ParameterValue = child_data.em_contact_mobile });
                lstParamters.Add(new ApiParameters() { ParameterName = "start_date", ParameterValue = child_data.start_date });
                //lstParamters.Add(new ApiParameters() { ParameterName = "end_date", ParameterValue = child_data.end_date });

                string json = await Utility.CallWebApi(lstParamters, url);
                if (json == null)
                {
                    await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                    return;
                }

                var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                if (oResult.response_status == "200")
                {
                    string child_id = oResult.data.ToString();
                    await OnAddHomeWays(child_id);
                    await OnAddChildImages(child_id);
                    await Utility.ShowNotification("", AppResources.msgChildAddedSuccessfully);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else await Utility.ShowNotification("", oResult.response_message + oResult.data.ToString().Replace("\n", "").Replace("\"", "").Replace("{", "").Replace("}", ""));
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

        private async Task<bool> OnAddHomeWays(string child_id)
        {
            try
            {
                string url = "api/parent_panel/add_child_home_way";
                List<ApiParameters> lstParamters;

                foreach (var oHomeRelation in lstHomeRelations)
                {
                    lstParamters = new List<ApiParameters>();
                    lstParamters.Add(new ApiParameters() { ParameterName = "lang", ParameterValue = Settings.Language });
                    lstParamters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = child_id });
                    lstParamters.Add(new ApiParameters() { ParameterName = "national_id", ParameterValue = oHomeRelation.national_id });
                    lstParamters.Add(new ApiParameters() { ParameterName = "relationship_id", ParameterValue = oHomeRelation.selected_relation.id });
                    lstParamters.Add(new ApiParameters() { ParameterName = "contact_name", ParameterValue = oHomeRelation.contact_name });
                    lstParamters.Add(new ApiParameters() { ParameterName = "contact_number", ParameterValue = oHomeRelation.contact_number });

                    string json = await Utility.CallWebApi(lstParamters, url);
                    if (json == null)
                    {
                        await Utility.ShowNotification("", AppResources.msgServerConnectionError);
                        return false;
                    }

                    var oResult = JsonConvert.DeserializeObject<GeneralModel>(json);
                    if (oResult.response_status != "200")
                    {
                        await Utility.ShowNotification("", oResult.response_message);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                await Utility.ShowNotification("", ex.Message);
                return false;
            }
        }

        private async Task<bool> OnAddChildImages_Old(string child_id)
        {
            try
            {
                List<ImageModel> lstImages;
                List<ApiParameters> lstParameters;
                string url = "api/parent_panel/add_child_profile_images";

                if (child_face != null)
                {
                    lstImages = new List<ImageModel>();
                    lstParameters = new List<ApiParameters>();
                    lstImages.Add(new ImageModel { image_content = child_face.image_content, extension = child_face.GetExtension() });
                    var images = JsonConvert.SerializeObject(lstImages);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_face", ParameterValue = images });
                    var result = await Utility.UploadFile(lstParameters, url);
                    if (result.response_status != "200") return false;
                }
                if (mother_id != null)
                {
                    lstImages = new List<ImageModel>();
                    lstParameters = new List<ApiParameters>();
                    lstImages.Add(new ImageModel { image_content = mother_id.image_content, extension = mother_id.GetExtension() });
                    var images = JsonConvert.SerializeObject(lstImages);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "mother_national_id_image", ParameterValue = images });
                    var result = await Utility.UploadFile(lstParameters, url);
                    if (result.response_status != "200") return false;
                }
                if (father_id != null)
                {
                    lstImages = new List<ImageModel>();
                    lstParameters = new List<ApiParameters>();
                    lstImages.Add(new ImageModel { image_content = father_id.image_content, extension = father_id.GetExtension() });
                    var images = JsonConvert.SerializeObject(lstImages);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "father_national_id_image", ParameterValue = images });
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

        private async Task<bool> OnAddChildImages(string child_id)
        {
            try
            {
                ImageModel oImage;
                List<ApiParameters> lstParameters;
                string url = "api/parent_panel/add_child_profile_images";

                if (child_face != null)
                {
                    lstParameters = new List<ApiParameters>();
                    oImage = new ImageModel() { image_content = child_face.image_content, extension = child_face.GetExtension() };
                    var images = JsonConvert.SerializeObject(oImage);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_face", ParameterValue = images });
                    var result = await Utility.UploadFile(lstParameters, url);
                    if (result.response_status != "200") return false;
                }
                if (mother_id != null)
                {
                    lstParameters = new List<ApiParameters>();
                    oImage = new ImageModel() { image_content = mother_id.image_content, extension = mother_id.GetExtension() };
                    var images = JsonConvert.SerializeObject(oImage);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "mother_national_id_image", ParameterValue = images });
                    var result = await Utility.UploadFile(lstParameters, url);
                    if (result.response_status != "200") return false;
                }
                if (father_id != null)
                {
                    lstParameters = new List<ApiParameters>();
                    oImage = new ImageModel { image_content = father_id.image_content, extension = father_id.GetExtension() };
                    var images = JsonConvert.SerializeObject(oImage);
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_profile_id", ParameterValue = child_id });
                    lstParameters.Add(new ApiParameters() { ParameterName = "father_national_id_image", ParameterValue = images });
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

        private async void OnTakePic(string _fileIndex)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                fileIndex = _fileIndex;

                //############### Yousef Mohamed (30-01-2019) ###############//
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
                //###########################################################//

                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                switch (fileIndex)
                {
                    case "1":
                        child_face_uploading = true;
                        break;
                    case "2":
                        mother_id_uploading = true;
                        break;
                    case "3":
                        father_id_uploading = true;
                        break;
                    default:
                        break;
                }

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
                    child_face_uploading = mother_id_uploading = father_id_uploading = false;
                    return;
                }

                var oImageData = new ImageModel
                {
                    image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream())),
                    file_path = Utility.GetFileName(file.Path)
                };

                switch (fileIndex)
                {
                    case "1":
                        child_face = oImageData;
                        break;
                    case "2":
                        mother_id = oImageData;
                        break;
                    case "3":
                        father_id = oImageData;
                        break;
                    default:
                        break;
                }

                child_face_uploading = mother_id_uploading = father_id_uploading = false;
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
                child_face_uploading = mother_id_uploading = father_id_uploading = false;
            }
        }

        private async void OnAddPic(string _fileIndex)
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                fileIndex = _fileIndex;

                //############### Yousef Mohamed (30-01-2019) ###############//
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
                //###########################################################//

                //await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Photo", ":( Pick photos not available.", "OK");
                    return;
                }

                switch (fileIndex)
                {
                    case "1":
                        child_face_uploading = true;
                        break;
                    case "2":
                        mother_id_uploading = true;
                        break;
                    case "3":
                        father_id_uploading = true;
                        break;
                    default:
                        break;
                }

                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Custom, CustomPhotoSize = 35 });
                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { CompressionQuality = 70  });
                //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Full, CompressionQuality = 30 });
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Small });

                if (file == null)
                {
                    child_face_uploading = mother_id_uploading = father_id_uploading = false;
                    return;
                }

                var oImageData = new ImageModel
                {
                    image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream())),
                    file_path = Utility.GetFileName(file.Path)
                };

                switch (fileIndex)
                {
                    case "1":
                        child_face = oImageData;
                        break;
                    case "2":
                        mother_id = oImageData;
                        break;
                    case "3":
                        father_id = oImageData;
                        break;
                    default:
                        break;
                }

                child_face_uploading = mother_id_uploading = father_id_uploading = false;
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
                child_face_uploading = mother_id_uploading = father_id_uploading = false;
            }
        }

        private void OnRemovePic(string fileIndex)
        {
            if (IsTapped) return;
            IsTapped = true;

            switch (fileIndex)
            {
                case "1":
                    child_face = null;
                    break;
                case "2":
                    mother_id = null;
                    break;
                case "3":
                    father_id = null;
                    break;
                default:
                    break;
            }
            IsTapped = false;
        }
        #endregion

    }
}