using Android.Content.PM;
using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlotMachineView : ContentPage
    {
        public SlotMachineView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        //during page close setting back to portrait
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
        }
        string numberToImageSource(int nr)
        {
            if(nr < 3)
            {
                return "CherrySlot.png";
            }
            else if(nr < 6)
            {
                return "GrapeSlot.png";
            }else if(nr < 9)
            {
                return "LemonSlot.png";
            }else if (nr < 12) 
            {
                return "OrangeSlot.png";
            }else if(nr < 14)
            {
                return "SevenSlot.png";
            }
            return "SpecialSlot.png";
        }
        void move(Object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;
            Random rnd = new Random();

            int randomSlot1 = rnd.Next(0, 15);
            int randomSlot2 = rnd.Next(0, 15);
            int randomSlot3 = rnd.Next(0, 15);

            slot1.IsVisible = false;
            slot2.IsVisible = false;
            slot3.IsVisible = false;

            spinSlot1.IsVisible = true;
            spinSlot2.IsVisible = true;
            spinSlot3.IsVisible = true;

            slot1.Source = numberToImageSource(randomSlot1);
            slot2.Source = numberToImageSource(randomSlot2);
            slot3.Source = numberToImageSource(randomSlot3);

            int counter = 0;
            double initialY = spinSlot1.TranslationY;
            Device.StartTimer(new TimeSpan(100), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    spinSlot1.TranslationY += 20;
                    spinSlot2.TranslationY += 20;
                    spinSlot3.TranslationY += 20;
                    counter += 1;
                });

                if(counter == 150)
                {
                    spinSlot1.IsVisible = false;
                    spinSlot2.IsVisible = false;
                    spinSlot3.IsVisible = false;

                    slot1.IsVisible = true;
                    slot2.IsVisible = true;
                    slot3.IsVisible = true;

                    spinSlot1.TranslationY = initialY;
                    spinSlot2.TranslationY = initialY;
                    spinSlot3.TranslationY = initialY;

                    (sender as Button).IsEnabled = true;    
                    return false;
                }
                return true; // runs again, or false to stop
            });
        }
    }
}