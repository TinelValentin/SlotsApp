using SevenSlots.Helpers;
using SevenSlots.Model;
using SevenSlots.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.ViewModel
{
    public class BlackjackViewModel : ViewModelBase
    {
        #region Enums...

        public enum GameState
        {
            PlayerBet,
            PlayerHit,
            PlayerStand,
            DealerStand,
            DealerDraw,
            PlayerBust,
            DealerBust,
            PlayerBlackJack,
            DealerBlackJack,
            Draw,
            PlayerWon,
            DealerWon,
            RoundOver,
            NewRound
        };

        #endregion

        #region Constructors...

        public BlackjackViewModel()
        {
            User = new User();

            if (Session.GeneralSettings != "")
            {
                User = JsonSerializer.Deserialize<User>(Session.GeneralSettings);
                _userService = DependencyService.Get<IUserService>();
            }
            _currentPlayer = new Player("John", User.Wallet);
            _player = _currentPlayer;

            InitializeCommands();
            InitializeGame();
        }

        #endregion

        #region Private Fields...

        private Player _player;
        private Player _currentPlayer;
        private Dealer _dealer;
        List<string> _cards = new List<string>()
        {
             "c01", "c02", "c03", "c04", "c05", "c06", "c07", "c08", "c09", "c10", "c11", "c12", "c13",
             "d01", "d02", "d03", "d04", "d05", "d06", "d07", "d08", "d09", "d10", "d11", "d12", "d13",
             "h01", "h02", "h03", "h04", "h05", "h06", "h07", "h08", "h09", "h10", "h11", "h12", "h13",
             "s01", "s02", "s03", "s04", "s05", "s06", "s07", "s08", "s09", "s10", "s11", "s12", "s13"
        };
        List<string> _selectedCards = new List<string>();
        private const int _initialDeal = 2;
        public GameState _currentGameState { get; set; }
        private bool _canClick;
        private bool _canIncreaseBet;
        private bool _canDecreaseBet;
        private bool _isDealVisible;
        private bool _isNextRoundVisible;
        private const int _aceAdjustment = 10;
        private const int _betModifier = 10;
        private IUserService _userService;

        #endregion

        #region Public Fields and Commands...

        public User User { get; set; }
        public Dealer Dealer
        {
            get { return _dealer; }
            set { _dealer = value; OnPropertyChanged(); }
        }
        public Player Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged(); }
        }
        public bool CanClick
        {
            get { return _canClick; }
            set { _canClick = value; OnPropertyChanged(); }
        }
        public bool CanIncreaseBet
        {
            get { return _canIncreaseBet; }
            set { _canIncreaseBet = value; OnPropertyChanged(); }
        }

        public bool CanDecreaseBet
        {
            get { return _canDecreaseBet; }
            set { _canDecreaseBet = value; OnPropertyChanged(); }
        }
        public bool IsDealVisible
        {
            get { return _isDealVisible; }
            set { _isDealVisible = value; OnPropertyChanged(); }
        }
        public bool IsNextRoundVisible
        {
            get { return _isNextRoundVisible; }
            set { _isNextRoundVisible = value; OnPropertyChanged(); }
        }

        public ICommand BetIncreaseCommand { get; set; }
        public ICommand BetDecreaseCommand { get; set; }
        public ICommand HitCommand { get; set; }
        public ICommand StandCommand { get; set; }
        public ICommand DealCommand { get; set; }
        public ICommand NextRoundCommand { get; set; }

        #endregion

        #region Private Methods...

        #region Initializers...

        private void InitializeCommands()
        {
            BetIncreaseCommand = new Command(BetIncrease);
            BetDecreaseCommand = new Command(BetDecrease);
            HitCommand = new Command(Hit);
            StandCommand = new Command(Stand);
            DealCommand = new Command(Deal);
            NextRoundCommand = new Command(NextRound);
        }

        private void InitializeGame()
        {
            _dealer = new Dealer("Mark");
            _currentGameState = GameState.PlayerBet;

            CanIncreaseBet = CanDecreaseBet = true;
            IsDealVisible = true;
            IsNextRoundVisible = false;
        }

        #endregion

        #region Base Commands...

        private void Hit()
        {
            if (_player.TotalBet >= 1)
            {
                _currentGameState = GameState.PlayerHit;
                string card = DealCard();
                int.TryParse(card.Substring(1).TrimStart('0'), out int cardValue1);

                if (cardValue1 >= 10)
                {
                    cardValue1 = 10;
                }

                _player.CardTotal += cardValue1;
                _player.Card.Add("../Images/Cards/" + card + ".bmp");
                CheckPlayerWinCondition();
                OnPropertyChanged(nameof(Player));
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Alert!", "You Must First Place A Bet!", "OK");
            }
        }

        private void Stand()
        {
            if (_player.TotalBet >= 1)
            {
                _currentGameState = GameState.PlayerStand;
                CanClick = Clickable();

                DealerDeal();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Alert!", "You Cannot Stand Without Betting First! Place A Bet!", "OK");
            }
        }

        private void Deal()
        {
            if (_player.TotalBet < 1)
            {
                App.Current.MainPage.DisplayAlert("Alert!", "You Must Place A Bet Before Dealing!", "OK");
                return;
            }

            CanIncreaseBet = false;
            CanDecreaseBet = false;
            Player.BankRoll -= Player.TotalBet;
            CanIncreaseBet = CanDecreaseBet = false;
            CanClick = Clickable();

            var value = DealInitalCards();
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
            OnPropertyChanged(nameof(Dealer));
            OnPropertyChanged(nameof(Player));

            if (_player.CardTotal == 21)
            {
                PlayerBlackJack();
            }
        }

        private void NextRound()
        {
            _currentGameState = GameState.PlayerBet;
            CanClick = false;

            if (_player.BankRoll == 0)
            {
                GameOver();
            }
            else
            {
                CanIncreaseBet = true;
                CanDecreaseBet = true;
                ResetBoard();
                OnPropertyChanged(nameof(Player));
                OnPropertyChanged(nameof(Dealer));
            }
        }

        #endregion

        #region Betting, Dealing, Shuffling..

        private (List<string> dCards, List<string> pCards) DealInitalCards()
        {
            List<string> dealerCards = new List<string>();
            List<string> playerCards = new List<string>();

            Random random = new Random();

            ReshuffleCards();

            for (int i = 0; i < _initialDeal; i++)
            {
                int r1 = random.Next(1, _cards.Count);
                dealerCards.Add(_cards[r1]);
                _selectedCards.Add(dealerCards[i]);
                _cards.Remove(dealerCards[i]);
            }

            for (int i = 0; i < _initialDeal; i++)
            {
                int r1 = random.Next(1, _cards.Count);
                playerCards.Add(_cards[r1]);
                _selectedCards.Add(playerCards[i]);
                _cards.Remove(playerCards[i]);
            }

            return (playerCards, dealerCards);
        }

        private string DealCard()
        {
            ReshuffleCards();
            Random random = new Random();

            int randomCardIndex = random.Next(1, _cards.Count);
            string dealtCard = _cards[randomCardIndex];
            _selectedCards.Add(_cards[randomCardIndex]);
            _cards.Remove(_cards[randomCardIndex]);

            return dealtCard;
        }

        private void DealerDeal()
        {
            _currentGameState = GameState.DealerDraw;
            _dealer.CardTotal = _dealer.HiddenCardTotal;
            _dealer.Card[0] = _dealer.HiddenCard;

            while (_dealer.CardTotal < 17)
            {
                string dCard = DealCard();
                int.TryParse(dCard.Substring(1).TrimStart('0'), out int dCardValue1);

                if (dCardValue1 >= 10)
                {
                    dCardValue1 = 10;
                }

                _dealer.CardTotal += dCardValue1;
                _dealer.Card.Add("../Images/Cards/" + dCard + ".bmp");
                CheckDealerWinCondition();
            }

            OnPropertyChanged(nameof(Dealer));
            OnPropertyChanged(nameof(Player));
            CheckGameWinCondition();
        }

        private void ReshuffleCards()
        {
            if (_cards.Count <= 4)
            {
                _cards.Clear();
                foreach (var card in _selectedCards)
                {
                    _cards.Add(card);
                }
                _selectedCards.Clear();
            }
        }

        private void BetIncrease(object param)
        {
            if(Player.BankRoll <= 0)
            {
                CanIncreaseBet = false;
            }
            else
            {
                Player.TotalBet += _betModifier;
            }
        }

        private void BetDecrease(object param)
        {
            if(Player.TotalBet <= 0)
            {
                CanDecreaseBet = false;
            }
            else
            {
                Player.TotalBet -= _betModifier;
            }
        }

        #endregion

        #region Win Conditions...

        private void CheckPlayerWinCondition()
        {
            if (_player.CardTotal >= 21)
            {
                foreach (var cards in _player.Card)
                {
                    if (cards.Substring(1).TrimStart('0') == "1")
                    {
                        _player.CardTotal -= _aceAdjustment;
                    }
                }
            }

            if (_player.CardTotal > 21)
            {
                PlayerBust();
            }

            if (_player.CardTotal == 21)
            {
                PlayerBlackJack();
            }
        }

        private void CheckDealerWinCondition()
        {
            if (_dealer.CardTotal >= 21)
            {
                foreach (var cards in _player.Card)
                {
                    if (cards.Substring(1).TrimStart('0') == "1")
                    {
                        _dealer.CardTotal -= _aceAdjustment;
                    }
                }
            }
        }

        private void CheckGameWinCondition()
        {
            if (_dealer.CardTotal > 21)
            {
                DealerBust();
            }
            else if (_dealer.CardTotal == 21)
            {
                DealerBlackJack();
            }
            else if (_player.CardTotal > _dealer.CardTotal)
            {
                PlayerWin();
            }
            else if (_player.CardTotal < _dealer.CardTotal)
            {
                DealerWin();
            }
            else if (_player.CardTotal == _dealer.CardTotal)
            {
                DrawGame();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Alert!", "Something Went Wrong! You Should Never See This Message.", "OK");
            }

            IsNextRoundVisible = Visible();

            OnPropertyChanged(nameof(Dealer));
            OnPropertyChanged(nameof(Player));
        }

        #endregion

        #region Game Results...

        private void PlayerBust()
        {
            _currentGameState = GameState.PlayerBust;
            App.Current.MainPage.DisplayAlert("Alert!", "You Lost! You Got Over 21!", "OK");

            CanClick = Clickable();
            IsNextRoundVisible = Visible();
            SaveData();
        }

        private void DealerBust()
        {
            _currentGameState = GameState.RoundOver;
            App.Current.MainPage.DisplayAlert("Alert!", "You Won! Dealer Got Over 21!", "OK");

            _player.TotalWinnings += _player.TotalBet * 2;
            _player.BankRoll += _player.TotalBet * 2;
            SaveData();
        }

        private void PlayerBlackJack()
        {
            if (_player.CardTotal == 21)
            {
                _currentGameState = GameState.PlayerBlackJack;
                App.Current.MainPage.DisplayAlert("Alert!", "Player Blackjack!", "OK");

                _player.TotalWinnings += _player.TotalBet * 2;
                _player.BankRoll += _player.TotalBet * 2;

                IsNextRoundVisible = Visible();
                CanClick = Clickable();
                SaveData();

                OnPropertyChanged(nameof(Player));
            }
        }

        private void DealerBlackJack()
        {
            _currentGameState = GameState.RoundOver;
            App.Current.MainPage.DisplayAlert("Alert!", "Dealer Blackjack!", "OK");

            SaveData();
        }

        private void PlayerWin()
        {
            _currentGameState = GameState.RoundOver;
            App.Current.MainPage.DisplayAlert("Alert!", "You Won!", "OK");

            _player.TotalWinnings += _player.TotalBet * 2;
            _player.BankRoll += _player.TotalBet * 2;
            SaveData();
        }

        private void DealerWin()
        {
            _currentGameState = GameState.RoundOver;
            App.Current.MainPage.DisplayAlert("Alert!", "Dealer Won!", "OK");

            SaveData();
        }

        private void DrawGame()
        {
            _currentGameState = GameState.RoundOver;
            App.Current.MainPage.DisplayAlert("Alert!", "It's A Draw!", "OK");

            _player.BankRoll += _player.TotalBet;
            SaveData();
        }

        #endregion

        #region Helper Functions...

        public bool Clickable()
        {
            if (_currentGameState == GameState.PlayerStand || _currentGameState == GameState.PlayerBust || _currentGameState == GameState.PlayerBlackJack)
            {
                return false;
            }
            else if (_currentGameState == GameState.PlayerBet)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Visible()
        {
            if (_currentGameState == GameState.RoundOver || _currentGameState == GameState.PlayerBlackJack || _currentGameState == GameState.PlayerBust)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ResetBoard()
        {
            _player.Card.Clear();
            InitializeGame();
        }

        public void GameOver()
        {
            App.Current.MainPage.DisplayAlert("Alert!", "Game Over!", "OK");
        }

        public void SaveData()
        {
            User.Wallet = _player.BankRoll;
            _userService.patchWallet(User.Id, User.Wallet);
        }

        #endregion

        #endregion
    }
}
