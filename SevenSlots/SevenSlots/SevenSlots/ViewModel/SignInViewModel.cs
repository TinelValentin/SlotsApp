using SevenSlots.Commands;
using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SevenSlots.ViewModel
{
    internal class SignInViewModel
    {
        User user = new User();
        public ICommand LoginCommand { get; }

        public User User { get { return user; } set { user = value; } }

        public SignInViewModel()
        {
            LoginCommand = new LoginCommand();
        }
    }
}
