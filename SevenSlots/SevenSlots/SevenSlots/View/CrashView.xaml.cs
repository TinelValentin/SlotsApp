using Android.Locations;
using SevenSlots.Model;
using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SevenSlots.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrashView : ContentPage
    {
        #region private members

        bool crash = false;
        decimal multiplier = 0.99m;
        decimal multiplierExponent = 0.01m;
        private int _countdown = 10;
        private Player _player;
        private bool _canBet;
        private bool canCashOut = false;
        private int _ownedMoney;
        private int _lastWin;
        private ObservableCollection<decimal> lastResults = new ObservableCollection<decimal>()
        {
            1.26m,1.25m
        };
        private List<int> multiplierLabelValues = new List<int>()
        {
            1,2,4
        };
        private List<int> secondsPassedValues = new List<int>()
        {
            0,1,2
        };


        const int _betModifier = 10;
        #endregion

        #region properties

        public ICommand BetIncreaseCommand { get; set; }
        public ICommand BetDecreaseCommand { get; set; }

        public ObservableCollection<decimal> LastResults
        {
            get { return lastResults; }
            set { lastResults = value; OnPropertyChanged(); }
        }

        public List<int> MultiplierLabelValues
        {
            get { return multiplierLabelValues; }
            set { multiplierLabelValues = value; OnPropertyChanged(); }
        }

        public List<int> SecondsPassedValues
        {
            get { return secondsPassedValues; }
            set { secondsPassedValues = value; OnPropertyChanged(); }
        }

        public decimal MultiplierValue
        {
            get { return decimal.Round(multiplier, 2); }
            set { multiplier = value; OnPropertyChanged(nameof(MultiplierValue)); }
        }

        public Player Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged(nameof(Player)); }
        }

        public bool CanBet
        {
            get { return _canBet; }
            set { _canBet = value; OnPropertyChanged(); }
        }

        public int OwnedMoney
        {
            get { return _ownedMoney; }
            set { _ownedMoney = value; OnPropertyChanged(); }
        }

        public int LastWin
        {
            get { return _lastWin; }
            set { _lastWin = value; OnPropertyChanged(); }
        }

        public int Countdown { get { return _countdown; } set { _countdown = value; OnPropertyChanged(); } }

        #endregion

        #region animation methods

        public async Task Fly()
        {
            await rocket.TranslateTo(230, -150, 8000, Easing.SinInOut);
        }

        public CrashView()
        {
            CanBet = false;
            BetIncreaseCommand = new Command(BetIncrease);
            BetDecreaseCommand = new Command(BetDecrease);
            InitializeComponent();
            BindingContext = this;
            OwnedMoney = 500;
            LastWin = 0;
            Player = new Player("Wiz", OwnedMoney);
            StartCrashAsync().GetAwaiter();
        }

        private void CountdownTimer()
        {
            Countdown = 11;
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                Countdown--;
                if (Countdown == 0)
                    return false;

                return true; // runs again, or false to stop
            });
        }

        private async Task RestartCrashAsync()
        {
            LastResults.Insert(0,decimal.Round(multiplier, 2));
            OnPropertyChanged(nameof(LastResults));
            Player.TotalBet = 0;
            canCashOut = false;
            CanBet = true;
            CrashFrame.Opacity = 0.2;
            CountdownTimer();
            PlaneCrashedText.IsVisible = true;
            crash = true;
            this.multiplierExponent = 0.01m;
            this.MultiplierValue = 0.99m;
            await rocket.TranslateTo(230, 0, 1000);
            await rocket.TranslateTo(1, 0, 100);
            await Task.Delay(TimeSpan.FromSeconds(10));
            crash = false;
            PlaneCrashedText.IsVisible = false;
            CrashFrame.Opacity = 1.0;
            CanBet = false;
            canCashOut = true;
            await StartCrashAsync();
        }

        private void UpdateMultiplierLabels()
        {
           
            Device.StartTimer(new TimeSpan(0, 0, 0,0,500), () =>
            {
                if(MultiplierValue>4)
                {
                    for (int i = 0; i < MultiplierLabelValues.Count; i++)
                    {
                        var power = MultiplierLabelValues.Count - i-1;
                        var value = (int)MultiplierValue - (int)Math.Pow(2,power)+1;
                        MultiplierLabelValues[i] = value;
                        OnPropertyChanged(nameof(MultiplierLabelValues));

                        if (crash)
                        {
                            MultiplierLabelValues = new List<int>()
                    {
                            1,2,4
                    };
                            return false;
                        }
                    }
                }
                
                return true;
            });

        }

        private void UpdateSecondsLabels()
        {

            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                for (int i = 0; i < SecondsPassedValues.Count; i++)
                {
                    SecondsPassedValues[i] += 1;
                    OnPropertyChanged(nameof(SecondsPassedValues));

                    if (crash)
                    {
                        SecondsPassedValues = new List<int>()
                    {
                            0,1,2
                    };
                        return false;
                    }
                }
                return true;
            });

        }

        public Task StartCrashAsync()
        {
            UpdateMultiplierLabels();
            UpdateSecondsLabels();
            bool stopThread = false;
#pragma warning disable CS4014 // This call is not awaited because we want the animation to run while the multiplier increases
            Fly();
#pragma warning restore CS4014 
            Random rnd = new Random();
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 50), () =>
            {
                // every 50 miliseconds the multiplier increases and the plane has a chance to crash
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
            return Task.CompletedTask;
        }
        private void BetIncrease(object param)
        {
            Player.TotalBet += _betModifier;
            OwnedMoney -= _betModifier;
            OnPropertyChanged(nameof(Player.TotalBet));
        }
        private void BetDecrease(object param)
        {
            if (Player.TotalBet <= 0)
            {
                return;
            }

            Player.TotalBet -= _betModifier;
            OwnedMoney += _betModifier;
            OnPropertyChanged(nameof(Player.TotalBet));
        }

        private void SetBet(object sender, EventArgs e)
        {
            if (!CanBet)
            {
                DisplayAlert("Alert", "You can't bet!", "OK").GetAwaiter();
                return;
            }
            CanBet = false;
        }

        private void CashOut(object sender, EventArgs e)
        {
            if (!canCashOut)
            {
                return;
            }

            OwnedMoney += (int)(Convert.ToDecimal(Player.TotalBet) * multiplier);
            LastWin = (int)(Convert.ToDecimal(Player.TotalBet) * multiplier);
            Player.TotalBet = 0;
        }
    }
    #endregion

}