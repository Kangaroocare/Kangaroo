using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kangaroo.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRenderer))]
namespace Kangaroo.Droid.Renderers
{
    public class MasterDetailPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.MasterDetailPageRenderer
    {
        public MasterDetailPageRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(VisualElement oldElement, VisualElement newElement)
        {
            base.OnElementChanged(oldElement, newElement);

            if (oldElement == null)
            {
                var mdp = (IMasterDetailPageController)newElement;
                if (mdp != null)
                {
                }
            }
        }

        //protected override void OnElementChanged(VisualElement oldElement, VisualElement newElement)
        //{
        //    base.OnElementChanged(oldElement, newElement);
        //    if (oldElement == null)
        //    {
        //        var mdp = (master)newElement;
        //        if(mdp!= null)
        //        {
        //            mdp.
        //        }
        //    }
        //}
    }
}