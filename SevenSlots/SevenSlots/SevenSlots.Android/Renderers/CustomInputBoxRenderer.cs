using Android.Content;
using SevenSlots.Controls;
using SevenSlots.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomInputBox), typeof(CustomInputBoxRenderer))]
namespace SevenSlots.Droid.Renderers
{
    /// <summary>
    /// This class overrides the input and controls
    /// the behaviour of the new one ONLY ON ANDROID
    /// TODO: Add for IOS
    /// </summary>
    public class CustomInputBoxRenderer : EntryRenderer
    {
        public CustomInputBoxRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //Configure native control (TextBox)
            if (Control != null)
            { 
                Control.SetTextColor(Android.Graphics.Color.Black);
                Control.Background = null;
            }

            // Configure Entry properties
            if (e.NewElement != null)
            {
                
            }
        }
    }
}