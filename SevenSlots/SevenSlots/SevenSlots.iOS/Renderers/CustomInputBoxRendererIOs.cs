using Foundation;
using SevenSlots.Controls;
using SevenSlots.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(CustomInputBox), typeof(CustomInputBoxRendererIOs))]
namespace SevenSlots.iOS.Renderers
{
    public class CustomInputBoxRendererIOs : EntryRenderer
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