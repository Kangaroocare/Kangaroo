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
    public class AddGalleryViewModel : BaseViewModel
    {

        #region Declarations
        private string _child_id;

        private ObservableCollection<ImageModel> _lstGalleryItems;
        #endregion

        #region Properties
        public string child_id
        {
            get { return _child_id; }
            set { SetProperty(ref _child_id, value); }
        }

        public ObservableCollection<ImageModel> lstGalleryItems
        {
            get { return _lstGalleryItems; }
            set { SetProperty(ref _lstGalleryItems, value); }
        }

        public ICommand AddGalleryCommand { get; }

        public ICommand TakePhotoCommand { get; private set; }

        public ICommand PickPhotoCommand { get; private set; }

        public ICommand PickVideoCommand { get; private set; }

        public ICommand RemovePhotoCommand { get; private set; }
        #endregion

        #region Functions
        public AddGalleryViewModel(INavigation navigation)
        {
            Navigation = navigation;

            lstGalleryItems = new ObservableCollection<ImageModel>();

            AddGalleryCommand = new Command(OnAddGallery);

            TakePhotoCommand = new Command(OnTakePhoto);
            PickPhotoCommand = new Command(OnPickPhoto);
            PickVideoCommand = new Command(OnPickVideo);
            RemovePhotoCommand = new Command<int>(OnRemovePhoto);
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
                oImageData.serial = lstGalleryItems.Count + 1;
                oImageData.image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream()));
                oImageData.file_path = Utility.GetFileName(file.Path);

                lstGalleryItems.Add(oImageData);
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
                oImageData.serial = lstGalleryItems.Count + 1;
                oImageData.image_content = Convert.ToBase64String(Utility.ReadToEnd(file.GetStream()));
                oImageData.file_path = Utility.GetFileName(file.Path);

                lstGalleryItems.Add(oImageData);
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
                oImageData.serial = lstGalleryItems.Count + 1;
                //oImageData.video_content = Utility.ReadToEnd(file.GetStream());
                oImageData.video_stream = file.GetStream();
                oImageData.file_path = Path.GetFileName(file.Path);

                lstGalleryItems.Add(oImageData);
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

            var oImage = lstGalleryItems.Where(a => a.serial == serial).FirstOrDefault();
            lstGalleryItems.Remove(oImage);
            IsTapped = false;
        }

        private async void OnAddGallery()
        {
            try
            {
                if (IsTapped) return;
                IsTapped = true;

                if (lstGalleryItems.Count == 0) { await Utility.ShowNotification("", AppResources.msgReqGalleryItems); return; }

                IsBusy = true;
                var is_images_uploaded = await OnAddGalleryImages();
                var is_videos_uploaded = await OnAddGalleryVideos();

                if (is_images_uploaded && is_videos_uploaded)
                {
                    await Utility.ShowNotification("", AppResources.msgGalleryAddedSuccessfully);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else await Utility.ShowNotification("", AppResources.msgAddGalleryError);
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

        private async Task<bool> OnAddGalleryImages()
        {
            try
            {
                string url = "api/teacher_panel/add_child_gallery_images";
                List<ApiParameters> lstParameters;
                List<ImageModel> lstImageToUpload;

                var lstImages = lstGalleryItems.Where(a => a.media_type == "Photo").ToList();
                foreach (var image in lstImages)
                {
                    var oImage = new ImageModel() { image_content = image.image_content, extension = image.GetExtension() };
                    lstImageToUpload = new List<ImageModel>() { oImage };
                    var images = JsonConvert.SerializeObject(lstImageToUpload);

                    lstParameters = new List<ApiParameters>();
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = child_id });
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

        private async Task<bool> OnAddGalleryVideos()
        {
            try
            {
                string url = "api/teacher_panel/add_child_gallery_videos";
                List<ApiParameters> lstParameters;

                var lstVideos = lstGalleryItems.Where(a => a.media_type == "Video").ToList();
                foreach (var video in lstVideos)
                {
                    lstParameters = new List<ApiParameters>();
                    lstParameters.Add(new ApiParameters() { ParameterName = "child_id", ParameterValue = child_id });
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
