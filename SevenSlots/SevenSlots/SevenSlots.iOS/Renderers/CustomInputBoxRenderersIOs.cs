using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using SevenSlots.Controls;
using SevenSlots.iOS.Renderers;

[assembly: ExportRenderer(typeof(CustomInputBox), typeof(CustomInputBoxRenderersIOs))]
namespace SevenSlots.iOS.Renderers
{
    public class CustomInputBoxRenderersIOs : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if (Control != null)
            {
                Control.Background = null;
            }

            // Configure Entry properties
            if (e.NewElement != null)
            {

            }
        }
    }
}