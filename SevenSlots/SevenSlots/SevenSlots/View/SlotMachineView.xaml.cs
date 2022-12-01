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
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
            await (BindingContext as SlotMachineViewModel).UpdateWallet();

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
            double baseWin = 15;

            if (img1 == "File: SpecialSlot.png" && img1 == img2 && img1 == img3)
            {
                return bet * baseWin * 5;
            }

            if (img1 == "File: SevenSlot.png" && img1 == img2 && img1 == img3)
            {
                return bet * baseWin * 2;
            }

            if((img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png") &&
               (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") ||
               (img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png") &&
               (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png") ||
               (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") &&
               (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png"))
            {
                return bet * baseWin;
            }

            if(img1 == img2 && (img3 == "File: SevenSlot.png" || img3 == "File: SpecialSlot.png") ||
                img1 == img3 && (img2 == "File: SevenSlot.png" || img2 == "File: SpecialSlot.png") ||
                img2 == img3 && (img1 == "File: SevenSlot.png" || img1 == "File: SpecialSlot.png"))
            {
                return bet * baseWin;
            }

            if(img1 == img2 && img2 == img3)
            {
                return bet * baseWin;
            }

            return 0;
        }

        private void changeSlotsVisibility(bool isVisibile)
        {
            slot1.IsVisible = isVisibile;
            slot2.IsVisible = isVisibile;
            slot3.IsVisible = isVisibile;
        }

        private void changeSpinSlotsVisibility(bool isVisibile)
        {
            spinSlot1.IsVisible = isVisibile;
            spinSlot2.IsVisible = isVisibile;
            spinSlot3.IsVisible = isVisibile;
        }

        private void spinSlots(double value)
        {
            spinSlot1.TranslationY += value;
            spinSlot2.TranslationY += value;
            spinSlot3.TranslationY += value;
        }

        private void resetSpinSlots(double initialPosition)
        {
            spinSlot1.TranslationY = initialPosition;
            spinSlot2.TranslationY = initialPosition;
            spinSlot3.TranslationY = initialPosition;
        }

        async void play(Object sender, EventArgs e)
        {
            SlotMachineViewModel bc = BindingContext as SlotMachineViewModel;

            if(bc.Bet > bc.Wallet)
            {
                await App.Current.MainPage.DisplayAlert("Poor alert",
                                  "You don't have enough money for that bet",
                                  "Ok");
                return;
            }

            (sender as Button).IsEnabled = false;

            bc.Wallet -= bc.Bet;
            bc.User.Wallet -= bc.Bet;

            Random rnd = new Random();
            int randomSlot1 = rnd.Next(0, 15);
            int randomSlot2 = rnd.Next(0, 15);
            int randomSlot3 = rnd.Next(0, 15);

            changeSlotsVisibility(false);

            changeSpinSlotsVisibility(true);

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
                    spinSlots(20);
                    counter += 1;
                });

                if(counter == 150)
                {
                    changeSpinSlotsVisibility(false);

                    changeSlotsVisibility(true);

                    resetSpinSlots(initialY);

                    bc.Win = win;
                    bc.Wallet += win;
                    bc.User.Wallet += win;

                    (sender as Button).IsEnabled = true;    

                    return false;
                }

                return true; // runs again, or false to stop
            });

            await bc.UpdateWallet();

            string userString = JsonSerializer.Serialize((BindingContext as SlotMachineViewModel).User);
            Session.GeneralSettings = userString;
        }
    }
}