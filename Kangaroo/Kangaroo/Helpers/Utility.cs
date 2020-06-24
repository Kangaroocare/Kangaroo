using Kangaroo.Controls;
using Kangaroo.Models;
using Kangaroo.Resources;
using Newtonsoft.Json;
//using Kangaroo.Services;
using Plugin.Connectivity;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Kangaroo.Helpers
{
    public static class Utility
    {

        #region Declarations
        public static readonly string ServerUrl = "https://kangaroocare-sa.com/";
        //public static readonly string ServerUrl = "https://dt-works.com/amd/kangaroo/";
        #endregion

        #region Properties

        #endregion

        #region Functions
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string FormatTextWithoutNewLines(string input)
        {
            return input.Replace("\r", " ").Replace("\n", ".");
        }

        public static void ChangeLanguage(string language)
        {
            if (Settings.Language == language) return;
            Settings.Language = language;
            App.Current.MainPage = new NavigationPage(new Views.HomePage());
        }

        // Converts unix date to C# date time
        public static DateTime ConvertUnixDateTime(string unixDate)
        {
            if (string.IsNullOrEmpty(unixDate)) return new DateTime();

            // Example of a UNIX timestamp for 11-29-2013 4:58:30
            double timestamp = double.Parse(unixDate);

            // Format our new DateTime object to start at the UNIX Epoch
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            // Add the timestamp (number of seconds since the Epoch) to be converted
            dateTime = dateTime.AddSeconds(timestamp);

            return dateTime;
        }

        public static async Task<bool> ShowNotification(string title, string message)
        {
            var oAlertPopUp = new AlertPopup(message);
            await Application.Current.MainPage.Navigation.PushPopupAsync(oAlertPopUp);
            return true;
        }

        public static async Task<bool> ShowNotification(string title, string message, int interval)
        {
            var oAlertPopUp = new AlertPopup(message, interval);
            await Application.Current.MainPage.Navigation.PushPopupAsync(oAlertPopUp);
            return true;
        }

        public static void PopPage()
        {
            var existingPages = Application.Current.MainPage.Navigation.NavigationStack.ToList();
            if (existingPages == null) return;
            Application.Current.MainPage.Navigation.RemovePage(existingPages[existingPages.Count - 1]);
        }

        // Return the HTTP client for the service address 
        public static HttpClient GetClient()
        {
            CookieContainer cookieContainer = new CookieContainer();
            Cookie cookie = new Cookie();
            Uri baseAddress = new Uri(Utility.ServerUrl);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var client = new HttpClient(handler) { BaseAddress = baseAddress };

            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value)) cookieContainer.Add(baseAddress, cookie);
            return client;
        }

        public static async Task<string> CallWebApi(List<ApiParameters> lstParameters, string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowNotification("", AppResources.msgNoInternetConnection);
                return null;
            }

            var parameters = new List<KeyValuePair<string, string>>();
            foreach (ApiParameters oClsApiParamters in lstParameters)
            {
                parameters.Add(new KeyValuePair<string, string>(oClsApiParamters.ParameterName, oClsApiParamters.ParameterValue));
            }

            HttpContent content = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("postman-token", "b05cb555-6691-4e83-92a8-c46abdb58633");
                    client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                    client.DefaultRequestHeaders.Add("authorization", "Basic YW1kOmthbmdhcm9vX2R0NGl0");

                    try
                    {
                        HttpResponseMessage loginResponse = await client.PostAsync(ServerUrl + url, content);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Report(ex);
            }
            return null;
        }

        public async static Task<string> CallWebApiJSON(object parameters, string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowNotification("", AppResources.msgNoInternetConnection);
                return null;
            }

            try
            {
                var uri = new Uri(ServerUrl + url);
                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await GetClient().PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    using (var responseContent = response.Content)
                    {
                        var result = responseContent.ReadAsStringAsync().Result;
                        return result;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Report(ex);
            }
            return null;
        }

        public static async Task<string> CallWebApiPost(List<ApiParameters> lstPramaters, string url)
        {
            try
            {
                string boundary = "---8d0f01e6b3b5dafaaadaad";
                MultipartFormDataContent form = new MultipartFormDataContent(boundary);

                foreach (var par in lstPramaters)
                {
                    form.Add(new StringContent(par.ParameterValue), par.ParameterName);
                }

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("authorization", "Basic YW1kOmthbmdhcm9vX2R0NGl0");

                HttpResponseMessage response = await client.PostAsync(ServerUrl + url, form);
                response.EnsureSuccessStatusCode();
                client.Dispose();
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception ex)
            {
                Log.Report(ex);
                return null;
            }
        }

        public static async Task<GeneralModel> UploadFile(List<ApiParameters> lstPramaters, string url)
        {
            try
            {
                string boundary = "---8d0f01e6b3b5dafaaadaad";
                MultipartFormDataContent form = new MultipartFormDataContent(boundary);

                foreach (var par in lstPramaters)
                {
                    form.Add(new StringContent(par.ParameterValue), par.ParameterName);
                }

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("authorization", "Basic YW1kOmthbmdhcm9vX2R0NGl0");

                HttpResponseMessage response = await client.PostAsync(ServerUrl + url, form);
                response.EnsureSuccessStatusCode();
                client.Dispose();
                string responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<GeneralModel>(responseContent);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Upload Error", AppResources.msgUploadImageError, "OK");
                Log.Report(ex);
                return null;
            }
        }

        public static async Task<GeneralModel> UploadVideo(List<ApiParameters> lstPramaters, Stream video, string videoKey, string fileName, string url)
        {
            try
            {
                string boundary = "---8d0f01e6b3b5dafaaadaad";
                MultipartFormDataContent form = new MultipartFormDataContent(boundary);

                foreach (var par in lstPramaters)
                {
                    form.Add(new StringContent(par.ParameterValue), par.ParameterName);
                }
                form.Add(new StreamContent(video), videoKey, fileName);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("authorization", "Basic YW1kOmthbmdhcm9vX2R0NGl0");

                HttpResponseMessage response = await client.PostAsync(ServerUrl + url, form);
                response.EnsureSuccessStatusCode();
                client.Dispose();
                string responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<GeneralModel>(responseContent);
                return apiResponse;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Upload Error", AppResources.msgUploadImageError, "OK");
                Log.Report(ex);
                return null;
            }
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static void ClearNavigationStack()
        {
            var existingPages = Application.Current.MainPage.Navigation.NavigationStack.ToList();
            for (int i = 1; i < existingPages.Count; i++)
            {
                Application.Current.MainPage.Navigation.RemovePage(existingPages[i]);
            }
        }

        public static decimal GetDecimal(Object fieldValue)
        {
            decimal nValue;
            if (fieldValue == null) return 0;
            decimal.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static double GetDouble(Object fieldValue)
        {
            double nValue;
            if (fieldValue == null) return 0;
            double.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static float GetFloat(Object fieldValue)
        {
            float nValue;
            if (fieldValue == null) return 0;
            float.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static int GetInteger(Object fieldValue)
        {
            int nValue;
            if (fieldValue == null) return 0;
            int.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static Guid GetGuid(Object fieldValue)
        {
            Guid guValue;
            if (fieldValue == null) return Guid.Empty;
            Guid.TryParse(fieldValue.ToString(), out guValue);
            return guValue;
        }

        public static long GetLong(Object fieldValue)
        {
            long nValue;
            if (fieldValue == null) return 0;
            long.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static bool GetBoolean(Object fieldValue)
        {
            bool nValue;
            if (fieldValue == null) return false;
            bool.TryParse(fieldValue.ToString(), out nValue);
            return nValue;
        }

        public static async Task ShareUri(string uri, string text, string title)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = title,
                Uri = uri,
                Text = text + " " + uri
            });
        }

        public static string GetFileName(string path)
        {
            var fullPath = path;
            var lastSlash = fullPath.LastIndexOf('/');
            return fullPath.Substring(lastSlash + 1);
        }

        public static async Task GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    Settings.LastKnownLocation = location;
                }
                else
                {
                    //Settings.LastKnownLocation = new Xamarin.Essentials.Location(21.422656, 39.835893);
                    //Settings.LastKnownLocation = new Xamarin.Essentials.Location(29.946361, 30.914154); // 6 October City
                    Settings.LastKnownLocation = new Xamarin.Essentials.Location(24.774265, 46.738586); // Riyadh City
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

    }
}

