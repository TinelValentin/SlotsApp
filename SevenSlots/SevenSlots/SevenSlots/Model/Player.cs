using SevenSlots.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenSlots.Model
{
    public class Player : ViewModelBase
    {
        #region BackingFields...

        private string _name;
        private double _bankRoll;
        private int _totalBet;
        private int _totalWinnings;
        private int? _cardTotal;
        private List<string> _card;
        private int _betAmount;

        #endregion

        #region Properties...
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public double BankRoll
        {
            get => _bankRoll;
            set { _bankRoll = value; OnPropertyChanged(nameof(BankRoll)); }
        }
        public int TotalBet
        {
            get => _totalBet;
            set { _totalBet = value; OnPropertyChanged(nameof(TotalBet)); }
        }
        public int TotalWinnings
        {
            get => _totalWinnings;
            set { _totalWinnings = value; OnPropertyChanged(nameof(TotalWinnings)); }
        }
        public int? CardTotal
        {
            get => _cardTotal;
            set { _cardTotal = value; OnPropertyChanged(nameof(CardTotal)); }
        }
        public List<string> Card
        {
            get => _card;
            set { _card = value; OnPropertyChanged(nameof(Card)); }
        }
        public int BetAmount
        {
            get => _betAmount;
            set { _betAmount = value; OnPropertyChanged(nameof(BetAmount)); }
        }

        #endregion

        #region Constructor...
        public Player(string name, double bankRoll, int totalBet = 0, int totalWin = 0, int betAmount = 0)
        {
            _name = name;
            _bankRoll = bankRoll;
            _totalBet = totalBet;
            _totalWinnings = totalWin;
            _betAmount = betAmount;
            this.Card = new List<string>();
        }

        #endregion
    }
}
