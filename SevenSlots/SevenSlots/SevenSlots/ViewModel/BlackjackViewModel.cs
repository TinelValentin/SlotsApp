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
        List<string> _cards = new List<string>()
        {
             "c01", "c02", "c03", "c04", "c05", "c06", "c07", "c08", "c09", "c10", "c11", "c12", "c13", 
             "d01", "d02", "d03", "d04", "d05", "d06", "d07", "d08", "d09", "d10", "d11", "d12", "d13", 
             "h01", "h02", "h03", "h04", "h05", "h06", "h07", "h08", "h09", "h10", "h11", "h12", "h13", 
             "s01", "s02", "s03", "s04", "s05", "s06", "s07", "s08", "s09", "s10", "s11", "s12", "s13"
        };
        List<string> selectedCards = new List<string>();
        private const int _initialDeal = 2;

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
        private (List<string> dCards, List<string> pCards) DealInitalCards()
        {
            List<string> dealerCards = new List<string>();
            List<string> playerCards = new List<string>();

            Random random = new Random();

            //reshuffleCards();

            for (int i = 0; i < _initialDeal; i++)
            {
                int r1 = random.Next(1, _cards.Count);
                dealerCards.Add(_cards[r1]);
                selectedCards.Add(dealerCards[i]);
                _cards.Remove(dealerCards[i]);
            }

            for (int i = 0; i < _initialDeal; i++)
            {
                int r1 = random.Next(1, _cards.Count);
                playerCards.Add(_cards[r1]);
                selectedCards.Add(playerCards[i]);
                _cards.Remove(playerCards[i]);
            }

            return (playerCards, dealerCards);
        }

        private void Deal()
        {
            _canBet = false;
            OnPropertyChanged(nameof(CanBet));

            _canClick = _gameBoard.Clickable();
            OnPropertyChanged(nameof(CanClick));

            var value = _gameBoard.DealInitalCards();
            _dealer.Card.Add("../Images/Cards/b1fv.bmp");
            _dealer.HiddenCard = ("../Images/Cards/" + value.dCards[0] + ".bmp");
            _dealer.Card.Add("../Images/Cards/" + value.dCards[1] + ".bmp");

            _player.Card.Add("../Images/Cards/" + value.pCards[0] + ".bmp");
            _player.Card.Add("../Images/Cards/" + value.pCards[1] + ".bmp");

            int.TryParse(value.dCards[0].Substring(1).TrimStart('0'), out int dCardValue1);
            int.TryParse(value.dCards[1].Substring(1).TrimStart('0'), out int dCardValue2);

            int.TryParse(value.pCards[0].Substring(1).TrimStart('0'), out int cardValue1);
            int.TryParse(value.pCards[1].Substring(1).TrimStart('0'), out int cardValue2);

            if (cardValue1 >= 10)
            {
                cardValue1 = 10;
            }

            if (cardValue2 >= 10)
            {
                cardValue2 = 10;
            }

            if (cardValue1 == 1 && cardValue2 <= 10)
            {
                cardValue1 = 11;
            }

            if (cardValue2 == 1 && cardValue1 <= 10)
            {
                cardValue2 = 11;
            }

            if (dCardValue1 >= 10)
            {
                dCardValue1 = 10;
            }

            if (dCardValue2 >= 10)
            {
                dCardValue2 = 10;
            }

            if (dCardValue1 == 1 && dCardValue2 <= 10)
            {
                dCardValue1 = 11;
            }

            if (dCardValue2 == 1 && dCardValue1 <= 10)
            {
                dCardValue2 = 11;
            }

            _player.CardTotal = cardValue1 + cardValue2;
            _dealer.HiddenCardTotal = dCardValue1 + dCardValue2;
            IsDealVisible = false;
            OnPropertyChanged(nameof(IsDealVisible));
            OnPropertyChanged(nameof(Dealer));
            OnPropertyChanged(nameof(Player));

            if (_player.CardTotal == 21)
            {
                PlayerBlackJack();
            }
        }
        #endregion
    }
}
