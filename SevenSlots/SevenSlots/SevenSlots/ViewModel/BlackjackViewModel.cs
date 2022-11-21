using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.ViewModel
{
    public class BlackjackViewModel : ViewModelBase
    {
        #region Constructors...
        public BlackjackViewModel()
        {
            TotalBet = 0;
            OwnedMoney = 12345;
            TotalWin = 0;
            InitializeCommands();
        }
        #endregion

        #region Private Fields...
        private int _totalBet;
        private int _ownedMoney;
        private int _totalWin;

        #endregion

        #region Public Fields and Commands...
        public int TotalBet
        {
            get { return _totalBet; }
            set { _totalBet = value; OnPropertyChanged(); }
        }
        public int OwnedMoney
        {
            get { return _ownedMoney; }
            set { _ownedMoney = value; OnPropertyChanged(); }
        }
        public int TotalWin
        {
            get { return _totalWin; }
            set { _totalWin = value; OnPropertyChanged(); }
        }

        public ICommand BetIncreaseCommand { get; set; }
        public ICommand BetDecreaseCommand { get; set; }
        #endregion

        #region Private Methods...
        private void InitializeCommands()
        {
            BetIncreaseCommand = new Command(BetIncrease);
            BetDecreaseCommand = new Command(BetDecrease);
        }
        private void BetIncrease(object param)
        {
            TotalBet += 10;
            OwnedMoney -= 10;
        }
        private void BetDecrease(object param)
        {
            TotalBet -= 10;
            OwnedMoney += 10;
        }
        #endregion
    }
}
