﻿using Android.Content.PM;
using SevenSlots.Model;
using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SevenSlots.Helpers;

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
            (BindingContext as SlotMachineViewModel).UpdateWallet();

            //Save the wallet locally as well
            string userString = JsonSerializer.Serialize((BindingContext as SlotMachineViewModel).User);
            Session.GeneralSettings = userString;
        }
        private string numberToImageSource(int nr)
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

        private double getWin(string img1, string img2, string img3)
        {
            double bet = (BindingContext as SlotMachineViewModel).Bet;

            if (img1 == "File: SpecialSlot.png" && img1 == img2 && img1 == img3)
            {
                return bet * 150;
            }

            if (img1 == "File: SevenSlot.png" && img1 == img2 && img1 == img3)
            {
                return bet * 60;
            }

            if((img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png") &&
               (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") ||
               (img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png") &&
               (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png") ||
               (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") &&
               (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png"))
            {
                return bet * 30;
            }

            if(img1 == img2 && (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png") ||
                img1 == img3 && (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") ||
                img2 == img3 && (img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png"))
            {
                return bet * 30;
            }

            if(img1 == img2 && img2 == img3)
            {
                return bet * 30;
            }

            return 0;
        }

        void move(Object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            SlotMachineViewModel bc = BindingContext as SlotMachineViewModel;

            bc.Wallet -= bc.Bet;
            bc.User.Wallet -= bc.Bet;

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

            double win = getWin(slot1.Source.ToString(),
                                slot2.Source.ToString(),
                                slot3.Source.ToString());


            int counter = 0;
            double initialY = spinSlot1.TranslationY;

            Device.StartTimer(new TimeSpan(100), () =>
            {
                Device.BeginInvokeOnMainThread( () =>
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

                    bc.Win = win;
                    bc.Wallet += win;
                    bc.User.Wallet += win;
                    bc.UpdateWallet();

                    (sender as Button).IsEnabled = true;    
                    return false;
                }

                return true; // runs again, or false to stop
            });

            bc.UpdateWallet();
        }
    }
}