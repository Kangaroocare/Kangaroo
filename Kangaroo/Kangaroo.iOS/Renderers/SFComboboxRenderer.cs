using Kangaroo.Controls;
using Kangaroo.iOS.Renderers;
using Syncfusion.XForms.ComboBox;
using Syncfusion.XForms.iOS.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSfComboBox), typeof(SFComboboxRenderer))]
namespace Kangaroo.iOS.Renderers
{
    public class SFComboboxRenderer : SfComboBoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SfComboBox> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.DropDownCornerRadius = 0;
                Control.Layer.CornerRadius = 0;
            }
        }
    }
}