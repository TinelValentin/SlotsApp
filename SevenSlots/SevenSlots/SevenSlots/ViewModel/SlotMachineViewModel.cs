using SevenSlots.Helpers;
using SevenSlots.Model;
using SevenSlots.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.ViewModel
{
    internal class SlotMachineViewModel : ViewModelBase
    {
        private User user = new User();
        public User User
        {
            get => user;
            set
            {
                user = value;
            }
        }

        private double wallet = 0;
        public double Wallet
        {
            get => wallet;
            set
            {
                wallet = value;
                OnPropertyChanged(nameof(WalletLabel));
            }
        }

        public string WalletLabel{ get => "Wallet: " + wallet.ToString(); }

        private double bet = 0;
        public double Bet
        {
            get => bet;
            set
            {
                bet = value;
                OnPropertyChanged(nameof(BetLabel));
            }
        }

        public string BetLabel { get => "Bet: " + bet.ToString(); }

        public ICommand BetIncreaseCommand { get; set; }
        public ICommand BetDecreaseCommand { get; set; }

        private double win = 0;
        public double Win
        {
            get => win;
            set
            {
                win = value;
                OnPropertyChanged(nameof(WinLabel));
            }
        }
        
        public string WinLabel { get => "Win: " + win.ToString(); }

        private int betChangeValue = 10;

        private void BetIncrease(object param)
        {
            Bet += betChangeValue;
        }

        private void BetDecrease(object param)
        {
            if (Bet >= betChangeValue) { 
                Bet -= betChangeValue;
            }
        }

        private IUserService userService;

        public async Task UpdateWallet()
        {
            await userService.patchWallet(user.Id, user.Wallet);
        }

        public SlotMachineViewModel()
        {
            if (Session.GeneralSettings != "")
            {
                user = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
            }
            if (user.Id != Guid.Empty)
            {
                BetIncreaseCommand = new Command(BetIncrease);
                BetDecreaseCommand = new Command(BetDecrease);
                userService = DependencyService.Get<IUserService>();

                Wallet = user.Wallet;
            }
            else
            {
                wallet = 0;
            }
        }
    }
}
