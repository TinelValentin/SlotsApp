using SevenSlots.Helpers;
using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SevenSlots.ViewModel
{
    internal class SlotMachineViewModel : ViewModelBase
    {
        private User user;
        private double wallet;
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

        public SlotMachineViewModel()
        {
            if (Session.GeneralSettings != "")
            {
                User user = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
            }

            if (user != null) { 
                wallet = user.Wallet;
            }
        }
    }
}
