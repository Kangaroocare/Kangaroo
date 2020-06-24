using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kangaroo.Resources
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {


        readonly CultureInfo ci = null;
        const string ResourceId = "Kangaroo.Resources.AppResources";

        public TranslateExtension()
        {
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                ci = AppResources.Culture;// DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {

            if (Text == null)
                return "";

            ResourceManager temp = new ResourceManager(ResourceId, typeof(AppResources).GetTypeInfo().Assembly);

            var translation = temp.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                          String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                          "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
