using Kangaroo.ViewModels;
using Kangaroo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Kangaroo.Models;

namespace Kangaroo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentView
    {

        #region Declarations
        private Map oMap;
        #endregion

        #region Functions
        public MapView(List<DayCareModel> lstDayCares)
        {
            InitializeComponent();
            CreateMap(lstDayCares);
        }

        private async Task CreateMap(List<DayCareModel> lstDayCares)
        {
            try
            {
                Position oPosition;
                var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (locationStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.LocationWhenInUse });
                    locationStatus = results[Permission.LocationWhenInUse];
                }

                if (locationStatus != PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Permissions Denied", "Unable change position", "OK");
                    oMap = new Map();
                }
                else
                {
                    oPosition = new Position(Settings.LastKnownLocation.Latitude, Settings.LastKnownLocation.Longitude);
                    oMap = new Map(MapSpan.FromCenterAndRadius(oPosition, Distance.FromMiles(0.3)))
                    {
                        IsShowingUser = true,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                }

                oMap.Pins.Clear();
                foreach (var oDayCare in lstDayCares)
                {
                    var oPin = new Pin();
                    oPosition = new Position(Utility.GetDouble(oDayCare.lat), Utility.GetDouble(oDayCare.lng));
                    oPin = new Pin
                    {
                        Type = PinType.Place,
                        Position = oPosition,
                        Label = oDayCare.daycare_name,
                        Address = "",
                        ClassId = "0"
                    };
                    oMap.Pins.Add(oPin);
                }

                slMap.Children.Clear();
                slMap.Children.Add(oMap);
            }
            catch(Exception ex)
            {
                Log.Report(ex);
            }
        }
        #endregion

        #region Events

        #endregion

    }
}