using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SevenSlots.ViewModel
{
    public class BlackjackViewModel
    {
        #region Constructors...
        public BlackjackViewModel()
        {
            InitializeCommands();
        }
        #endregion


        #region Fields...
        public ICommand BetIncreaseCommand { get; set; }
        #endregion


        #region Private Methods...
        private void InitializeCommands()
        {
            BetIncreaseCommand = new Command(BetIncrease);
        }

        private void BetIncrease()
        {

        }
        #endregion
    }
}
