using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrashView : ContentPage
    {

        bool crash = false;
        decimal multiplier = 0.99m;
        decimal multiplierExponent = 0.01m;

        public decimal MultiplierValue { 
            get { return decimal.Round(multiplier,2); } 
            set { multiplier = value;OnPropertyChanged(nameof(MultiplierValue)); } }

        public async Task Fly()
        {
           
            await rocket.TranslateTo(230, -150, 8000,Easing.SinInOut);

        }

        public CrashView()
        {
            InitializeComponent();
            BindingContext = this;
            StartCrashAsync().GetAwaiter();
        }

        private async Task RestartCrashAsync()
        {
            crash = !crash;
            this.multiplierExponent = 0.01m;
            this.MultiplierValue = 0.99m;
            await rocket.TranslateTo(230, 0, 1000);
            await rocket.TranslateTo(1, 0, 100);
            await Task.Delay(TimeSpan.FromSeconds(5));
            await StartCrashAsync();
        }

        public async Task StartCrashAsync()
        {
            bool stopThread = false;
            Fly();
            Random rnd = new Random();
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 50), () =>
            {
                // do something every 50 miliseconds
                Device.BeginInvokeOnMainThread(() =>
                {
                    int randomNumber = rnd.Next(0, 10000);
                    MultiplierValue += multiplierExponent;
                    multiplierExponent += 0.00025m;
                    if (randomNumber < 150)
                         crash = true;
                });
                if (crash)
                {
                    stopThread = true;

                    RestartCrashAsync().GetAwaiter();
                }
                return !stopThread; // when it will crash the timer will stop
            });
            
        }
    }
}