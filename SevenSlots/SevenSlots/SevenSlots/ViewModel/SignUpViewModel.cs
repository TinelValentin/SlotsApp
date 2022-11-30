using SevenSlots.Commands;
using SevenSlots.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SevenSlots.ViewModel
{
    internal class SignUpViewModel
    {
        User user = new User();
        public ICommand RegisterCommand { get; }
        public ICommand RegisterCancelCommand { get; }

        public User User { get { return user; } set { user = value; } }

        public SignUpViewModel()
        {
            RegisterCommand = new RegisterCommand();
            RegisterCancelCommand = new RegisterCancelCommand();
        }
    }
}
